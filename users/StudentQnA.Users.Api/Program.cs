using Microsoft.EntityFrameworkCore;
using StudentQnA.Users.Api.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

//Docker secrets
//var dbUser = File.ReadAllText("/run/secrets/postgres_user").Trim();
//var dbPassword = File.ReadAllText("/run/secrets/postgres_password").Trim();

//Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS Policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://4.178.10.200") // React dev servers
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

//Health check
builder.Services.AddHealthChecks();

var app = builder.Build();

//CORS policy
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
