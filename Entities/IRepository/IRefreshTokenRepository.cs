using Entities.DTO;
using Entities.Models;

namespace Entities.IRepository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        void Update(RefreshToken refreshToken);
    }
}
