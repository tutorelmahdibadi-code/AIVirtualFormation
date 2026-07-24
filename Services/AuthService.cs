using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VIRTUAL_CLASSE_FORMATION.Data;
using VIRTUAL_CLASSE_FORMATION.Dtos;
using VIRTUAL_CLASSE_FORMATION.Models;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;
        private readonly PasswordHasher<Utilisateur> passwordHasher = new();

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            bool emailExiste = await context.Utilisateurs.AnyAsync(u => u.Email == dto.Email);
            if (emailExiste)
                throw new InvalidOperationException("Cet email est deja utilise.");

            var utilisateur = new Utilisateur
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Role = dto.Role
            };
            utilisateur.PasswordHash = passwordHasher.HashPassword(utilisateur, dto.Password);

            context.Utilisateurs.Add(utilisateur);
            await context.SaveChangesAsync();

            return GenererReponse(utilisateur);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var utilisateur = await context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (utilisateur == null)
                return null;

            var resultat = passwordHasher.VerifyHashedPassword(utilisateur, utilisateur.PasswordHash, dto.Password);
            if (resultat == PasswordVerificationResult.Failed)
                return null;

            return GenererReponse(utilisateur);
        }

        private AuthResponseDto GenererReponse(Utilisateur utilisateur)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
                new(ClaimTypes.Email, utilisateur.Email),
                new(ClaimTypes.Name, $"{utilisateur.Prenom} {utilisateur.Nom}"),
                new(ClaimTypes.Role, utilisateur.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpirationMinutes"]!)),
                signingCredentials: credentials
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Nom = $"{utilisateur.Prenom} {utilisateur.Nom}",
                Email = utilisateur.Email,
                Role = utilisateur.Role
            };
        }
    }
}
