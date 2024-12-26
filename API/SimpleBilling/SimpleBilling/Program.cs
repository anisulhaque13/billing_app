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

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Kestrel
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8090); 
});



// Build the application
var app = builder.Build();

app.MapGet("/health", () => Results.Ok("Healthy"));

// Apply database migrations
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
app.UseHttpsRedirection();
app.UseCors(); // Apply the default CORS policy
app.UseAuthorization();
app.MapControllers();
app.Run();
