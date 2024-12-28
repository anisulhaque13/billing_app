using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBilling.Data;

var builder = WebApplication.CreateBuilder(args);

// Enable logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext
builder.Services.AddDbContext<BillingDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BillingDB")));

// Define CORS policy
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://23.251.152.29", "https://23.251.152.29") // Add your client app's URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Load Kestrel certificate configuration


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // HTTP
});

var app = builder.Build();

// Apply database migrations with retry logic
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BillingDBContext>();

    const int maxRetries = 5;
    int retryCount = 0;

    while (retryCount < maxRetries)
    {
        try
        {
            context.Database.Migrate();
            Console.WriteLine("Database migration applied successfully.");
            break;
        }
        catch (SqlException ex)
        {
            retryCount++;
            Console.WriteLine($"Attempt {retryCount} failed to connect to the database. Exception: {ex.Message}");
            if (retryCount >= maxRetries)
            {
                Console.WriteLine("Max retries reached. Could not connect to the database.");
                throw;
            }
            Thread.Sleep(5000); // Wait 5 seconds before retrying
        }
    }
}

// Configure middleware
app.UseCors(MyAllowSpecificOrigins); // Apply CORS policy
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();