using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;
using System.Security.Cryptography;
using Entities.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        public TokenRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<string> GenAccessToken(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>

                        {
                             new Claim(ClaimTypes.Name, user.UserName),
                             new Claim(ClaimTypes.NameIdentifier, user.Id),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             //new Claim(ClaimTypes.Role, userRoles.FirstOrDefault()),
                        };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //Token
            JwtSecurityToken myToken = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidVudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signingCred
            );

            return new JwtSecurityTokenHandler().WriteToken(myToken);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<(string Token, DateTime Expiry)> CreateRefreshToken(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = GenerateRefreshToken();

            var refreshToken = new RefreshToken()
            {
                Token = token,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                User = user
            };
            try
            {
                _context.RefreshToken.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                DeleteOldRefreshTokens(user, refreshToken);

                return (token, refreshToken.Expires);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating refresh token", ex);
            }
        }

        public void DeleteOldRefreshTokens(ApplicationUser user, RefreshToken latest)
        {
            var old = _context.RefreshToken.Where(z => z.UserId == user.Id && z.Id != latest.Id).ToList();
            if (old != null)
            {
                _context.RefreshToken.RemoveRange(old);
                _context.SaveChangesAsync();
            }
        }
        public async Task<ResponseDto?> GetRefreshToken(string request)
        {
            if (string.IsNullOrEmpty(request)) throw new ArgumentNullException(nameof(request));
            RefreshToken? refreshToken = await _context.RefreshToken.Include(z => z.User)
                 .FirstOrDefaultAsync(c => c.Token == request);

            if (refreshToken?.Expires < DateTime.UtcNow || refreshToken == null)
            {
                throw new ApplicationException("refresh token expired");
            }

            string accessToken = await GenAccessToken(refreshToken.User);
            refreshToken.Token = GenerateRefreshToken();
            refreshToken.Expires = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new ResponseDto()
            {
                token = accessToken,
                refreshToken = refreshToken.Token,
                refreshTokenExpiration = refreshToken.Expires,
            };

        }
    }
}
