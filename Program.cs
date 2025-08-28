using System.Text;
using AutoMapper;
using IdentityVerification.Api.Data;
using IdentityVerification.Api.Infrastructure;
using IdentityVerification.Api.Mapping;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IdentityVerification.Api.Security;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// EF Core (MySQL)
// -----------------------------
var conn = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
});

// -----------------------------
// AutoMapper
// -----------------------------
builder.Services.AddAutoMapper(typeof(MappingProfile));

// -----------------------------
// Repositories
// -----------------------------
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// -----------------------------
// Services
// -----------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IFormFieldService, FormFieldService>();
builder.Services.AddScoped<IFieldCategoryService, FieldCategoryService>();
builder.Services.AddScoped<ITemplateFormFieldService, TemplateFormFieldService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IResponseSubmissionService, ResponseSubmissionService>();
builder.Services.AddScoped<IUserResponseService, UserResponseService>();
builder.Services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();

// -----------------------------
// JWT
// -----------------------------
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;
builder.Services.AddSingleton(new JwtTokenGenerator(
    jwtKey, jwtIssuer, jwtAudience,
    int.Parse(builder.Configuration["Jwt:ExpiresMinutes"] ?? "120")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// -----------------------------
// Controllers
// -----------------------------
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Keep property names as declared in DTOs/entities.
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// -----------------------------
// Swagger + JWT support
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Identity Verification API",
        Version = "v1"
    });

    // Define the Bearer scheme and reference it so it’s used by SecurityRequirement
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Enter only your token below (no \"Bearer \" prefix).",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    // Register the scheme
    c.AddSecurityDefinition("Bearer", securityScheme);

    // Require Bearer token for all operations by default.
    // After you click Authorize and provide a token, Swagger will include the header automatically.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "IdentityVerification.Api.xml");
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// -----------------------------
// HTTP pipeline
// -----------------------------

// If you prefer Swagger in all environments, move these two lines outside the IsDevelopment check.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
