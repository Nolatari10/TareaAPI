using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareaAPI.Data;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;
using BCrypt.Net; // Importante para el hashing
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace TareaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        // 1. Validar que el usuario no exista
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest("El usuario ya existe.");
        }

        // 2. Crear el usuario y HASHEAR la contraseña
        // BCrypt genera el hash seguro automáticamente
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash // Guardamos el hash, NUNCA el texto plano
        };

        // 3. Guardar en BD
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuario registrado exitosamente.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        // 1. Buscar usuario
        var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
        {
            return BadRequest("Usuario o contraseña no incorrectos.");
        }

        //Genera token
        string token = CreateToken(user);
        return Ok(token);
        // 2. Verificar contraseña

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Contraseña incorrecta.");
        }

        //generate JWT token here in future
        return Ok("Inicio de sesión exitoso.");
    }
    
    private string CreateToken(User user)
    {
        //Define claims (user data)
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Get the secret key from configuration or appsettings
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("Jwt:Key").Value!));

        // Create signing credentials (key and algorithm hmacsha256)
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        // Create the token
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1), //expires in 1 hour
            signingCredentials: creds);
            
        // Write the token to a string
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
        
}
