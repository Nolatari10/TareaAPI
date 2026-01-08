using Microsoft.EntityFrameworkCore;
using TareaAPI.Data;
using FluentValidation;
using TareaAPI.Validators;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Para Swagger
using Swashbuckle.AspNetCore;
using TareaAPI.Repositories;
using TareaAPI.Models.DTOs; // Opcional, pero ayuda en Swagger

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Repositories
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
//Validators
builder.Services.AddValidatorsFromAssemblyContaining<TareaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoriaValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<TareaUpdateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TareaCreateDto>();

builder.Services.AddValidatorsFromAssemblyContaining<CategoriaUpdateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoriaCreateDto>();
var connectionString = builder.Configuration
.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
    
// SQLite - archivo local tareas.db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TareaAPI", Version = "v1" });  

});

//JWT Configuration Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = false, //valdidate this on production
        ValidateAudience = false, // validate this on production
    };
});

//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();


// Migrate database on startup (development only)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // Crea DB y tablas automáticamente
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication(); // verifica el token JWT
app.UseAuthorization(); // verifica roles/permisos
app.MapControllers();

app.Run();
