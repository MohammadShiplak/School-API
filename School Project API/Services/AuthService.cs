using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School_Project_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 


        }





        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
         // find user by email
            var user=await _context.Users.SingleOrDefaultAsync(u=>u.Email == loginDTO.Email);

            // check user exists and password is correct

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
                return null;

            if (!user.IsActive)
                return null;


            // Step 4: Build claims (info inside the token)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Step 5: Get secret key from appsettings.json
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Step 6: Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: creds
            );

            // Step 7: Return token as string
            return new JwtSecurityTokenHandler().WriteToken(token);



        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
           
            var exists = await _context.Users.AnyAsync(u=>u.Email == registerDTO.Email);

            if (exists)
                return false;

            var newUser = new User
            {

                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Role = registerDTO.Role,
                IsActive = registerDTO.IsActive,   
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();  
            return true;   
        }
    }
}
