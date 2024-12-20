using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBilling.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext
builder.Services.AddDbContext<BillingDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BillingDB")));

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // HTTP
    //options.ListenAnyIP(8081, listenOptions =>
    //{
    //    listenOptions.UseHttps("/https/https-dev-cert.pfx", "121");
    //});
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();
app.UseCors("AllowAll");
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
app.UseAuthorization();
app.MapControllers();
app.Run();
