using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniStoreDemo.Api.ExceptionHandling;
using MiniStoreDemo.Application.Abstractions.Authentication;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Application.Services;
using MiniStoreDemo.Domain.Entities;
using MiniStoreDemo.Infrastructure.Authentication;
using MiniStoreDemo.Infrastructure.Data;
using MiniStoreDemo.Infrastructure.Persistence;
using MiniStoreDemo.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("MiniStoreDemoConnStr") ?? throw new InvalidOperationException("Connection string 'MiniStoreDemoConnStr' was not found.");

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");


// Configure database access with EF Core
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));

builder.Services.AddControllers();

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Register application and data access dependencies
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
builder.Services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Configure JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        ValidateAudience = true,
        ValidAudience = jwtAudience,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Configure authorization policies
builder.Services.AddAuthorizationBuilder().AddPolicy("ManageProducts", policy => policy.RequireRole("Admin"));


// Configure log file paths
string logFolder = builder.Configuration["LoggingSettings:LogFolder"] ?? throw new InvalidOperationException("LoggingSettings > LogFolder is not configured.");
string logFileNameInfo = builder.Configuration["LoggingSettings:LogFileNameInfo"] ?? throw new InvalidOperationException("LoggingSettings > LogFileNameInfo is not configured.");
string logFileNameError = builder.Configuration["LoggingSettings:LogFileNameError"] ?? throw new InvalidOperationException("LoggingSettings > LogFileNameError is not configured.");

string logPathInfo = Path.Combine(AppContext.BaseDirectory, logFolder, logFileNameInfo);
string logPathError = Path.Combine(AppContext.BaseDirectory, logFolder, logFileNameError);

// TODO: Add dedicated audit logging
//string logFileNameAudit = builder.Configuration["LoggingSettings:LogFileNameAudit"] ?? throw new InvalidOperationException("LoggingSettings > LogFileNameAudit is not configured.");
//string logPathAudit = Path.Combine(AppContext.BaseDirectory, logFolder, logFileNameAudit);



builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        //.WriteTo.File(path: logPath, rollingInterval: RollingInterval.Day)
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(logEvent => logEvent.Level >= LogEventLevel.Information && logEvent.Level < LogEventLevel.Error)
            .WriteTo.File(
                logPathInfo,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
        )
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(logEvent => logEvent.Level >= LogEventLevel.Error)
            .WriteTo.File(
                logPathError,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 90));
});

// Configure centralized exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure Swagger documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var httpsUrl = app.Urls.FirstOrDefault(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

    Console.WriteLine($"Base URL: {httpsUrl}");
});

// Handle unhandled exceptions centrally
app.UseExceptionHandler();

// Log HTTP request completion information
app.UseSerilogRequestLogging();

// Expose Swagger UI
//if (app.Environment.IsDevelopment())
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Authenticate the user before evaluating authorization policies
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
