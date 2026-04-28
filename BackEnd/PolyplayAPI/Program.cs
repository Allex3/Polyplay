using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;

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


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// adding the DB Contexts
builder.Services.AddDbContext<GameDbContext>(options => options.UseInMemoryDatabase("Games"));

var app = builder.Build();

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
