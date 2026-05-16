using Microsoft.EntityFrameworkCore;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;
using System.Text;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ── Step 1: DbContext ─────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// ── Step 2: Register Services ─────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IAccessCardService, AccessCardService>();
builder.Services.AddScoped<ICourseService, CourseService>();

// ── Step 3: JWT Authentication ────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

           
        };
    });

builder.Services.AddAuthorization(options =>
{
    
});
// ── Step 4: Controllers ───────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Step 5: Swagger WITH JWT ──────────────────────────────────
// ✅ FIX 3: Only ONE AddSwaggerGen call with JWT config inside
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, // ← Http not ApiKey
        Scheme = "bearer",                // ← must be lowercase
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token only — Swagger adds 'Bearer ' prefix automatically"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ── Step 6: Middleware Pipeline (ORDER MATTERS!) ──────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ← MUST be before UseAuthorization
app.UseAuthorization();
app.MapControllers();
app.Run();