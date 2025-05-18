using DataAccess.Repository;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Structure
{
    public class GenerateAccessToken
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly UnitOfWork _unitOfWork;
        public GenerateAccessToken(UserManager<ApplicationUser> userManager, UnitOfWork _unitOfWork, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
            this._unitOfWork = _unitOfWork;
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

        private string GenerateRefreshToken()
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
                _unitOfWork.RefreshToken.Add(refreshToken);
                await _unitOfWork.Complete();

                return (token, refreshToken.Expires);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating refresh token", ex);
            }
        }

        public async Task<ResponseDto?> GetRefreshToken(string request)
        {
            if (string.IsNullOrEmpty(request)) throw new ArgumentNullException(nameof(request));
            RefreshToken? refreshToken = await _unitOfWork.RefreshToken
                 .GetFirstorDefault(c => c.Token == request, IncludeWord: "User");

            if (refreshToken?.Expires < DateTime.UtcNow || refreshToken == null)
            {
                throw new ApplicationException("refresh token expired");
            }

            string accessToken = await GenAccessToken(refreshToken.User);
            refreshToken.Token = GenerateRefreshToken();
            refreshToken.Expires = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.Complete();

            return new ResponseDto()
            {
                token = accessToken,
                refreshToken = refreshToken.Token,
                refreshTokenExpiration = refreshToken.Expires,
            };

        }
    }
}
