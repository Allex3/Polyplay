using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Chats;
using PolyplayAPI.Services;
using System.Collections.Concurrent;
using System.Net.WebSockets;


static string getConnString()
{
    var configBuilder = new ConfigurationBuilder();
    configBuilder.AddJsonFile("appsettings.json");

    IConfiguration configuration = configBuilder.Build();
    return configuration.GetConnectionString("DefaultConnection");
}

string connString = getConnString();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueSite", policy => policy
        .WithOrigins("http://localhost:5173", "https://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});



// add model validation for more legible HTTP failure responses to use in the front-end
builder.Services.AddScoped<ValidationFilterAttribute>();
// now disable the automatic validation (that returns the errors in errors, because now we will use a filter)
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

// add DI for MongoDB Chat database
builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase")); // maps to fields of same names as the properties, populated by DI
builder.Services.AddSingleton<GeneralChatService>();
builder.Services.AddSingleton<ConcurrentDictionary<string, WebSocket>>(); // for general chat... testing


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// adding the DB Context for the polyplay database
builder.Services.AddDbContext<PolyplayDbContext>(options => options.UseSqlServer(connString));

var app = builder.Build();

app.UseWebSockets(); // use web sockets, all origins allowed, ping every 2 minutes by default


app.UseCors("VueSite");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
