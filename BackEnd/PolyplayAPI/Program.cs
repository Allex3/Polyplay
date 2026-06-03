using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PolyplayAPI;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Auth;
using PolyplayAPI.Models.Chats;
using PolyplayAPI.Services;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net.WebSockets;
using System.Text;
using System.Threading.RateLimiting;


var configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json");

IConfiguration configuration = configBuilder.Build();

string connString = configuration.GetConnectionString("DefaultConnection");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueSite", policy => policy
        .WithOrigins("https://172.30.248.197:8080", "https://172.30.248.197", "https://localhost:8080", "https://192.168.1.128:8080", "https://192.168.1.128", "https://192.168.1.128:8080/", "https://192.168.1.128/")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// prevent DoS by rate limiting requests per user:
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true, // refresh counter
                PermitLimit = 20, // 20 requests per user or IP (anonymous)
                QueueLimit = 0, // disable queue mechanism, just deny
                Window = TimeSpan.FromMinutes(1), // per minute reset
            })
    );
    // options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // too simple
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers["Retry-After"] = "60";

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Wait 1 minute.. or more", cancellationToken);

        // Optional logging
        //logger.LogWarning("Rate limit exceeded for IP: {IpAddress}",
          //  context.HttpContext.Connection.RemoteIpAddress);
    };
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

// db context (without identity)

builder.Services.AddDbContext<PolyplayDbContext>(options =>
    options.UseSqlServer(connString));

// Add DI

// Add controller services to the container.
builder.Services.AddControllers();

// add Token Validation Parameters
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:Secret") ?? "uhhhwelpsecretkeynotwork?")),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),

    ValidateAudience = true,
    ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),

    ValidateLifetime = true
};

builder.Services.AddSingleton(tokenValidationParameters);

// ASP.NET Core Identity

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<PolyplayDbContext>()
    .AddDefaultTokenProviders(); // for reset pass, email, 2FA

// Add Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme; // when challenge => go check for jwt then redirect to login idk
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
// Add JWT Bearer to authentication
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;  // save jwt token
        options.RequireHttpsMetadata = true; // only use https
        options.TokenValidationParameters = tokenValidationParameters;

    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// adding the DB Context for the polyplay database

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("VueSite");

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets(); // use web sockets, all origins allowed, ping every 2 minutes by default

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

/*
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
*/

app.UseRateLimiter();

app.MapControllers();

// after everything, initialize the Identity part of the db (Roles)

DbInitializer.SeedRoles(app).Wait();

app.Run();
