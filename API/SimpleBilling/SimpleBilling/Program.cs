using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBilling.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext
builder.Services.AddDbContext<BillingDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BillingDB")));

// Configure Kestrel to use the PORT environment variable provided by Cloud Run
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port)); // Bind to the dynamic port
});

// Define CORS policy
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(
            builder.Configuration["AllowedOrigins"] ?? "http://35.198.101.145/") // Read from configuration or default
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add a basic health check service
builder.Services.AddHealthChecks();

var app = builder.Build();

// Use CORS policy
app.UseCors(policy =>
    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .SetIsOriginAllowed(origin => true) // Allow requests from any origin
          .AllowCredentials());

// Apply migrations with retry logic
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
            Console.WriteLine($"Attempt {retryCount} failed to connect to the database. Retrying in 5 seconds...");
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
app.UseSwagger();
app.UseSwaggerUI();

// Use health checks middleware
app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
