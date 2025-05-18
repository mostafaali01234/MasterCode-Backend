using Entities.DTO;
using Entities.Models;

namespace Entities.IRepository
{
    public interface ITokenRepository
    {
        Task<string> GenAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        Task<(string Token, DateTime Expiry)> CreateRefreshToken(ApplicationUser user);
        Task<ResponseDto?> GetRefreshToken(string request);
    }
}
