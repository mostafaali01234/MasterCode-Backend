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

namespace DataAccess.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(RefreshToken refreshToken)
        {
            var RefreshTokenInDb = _context.RefreshToken.FirstOrDefault(x => x.Id == refreshToken.Id);
            if (RefreshTokenInDb != null)
            {
                RefreshTokenInDb.Token = refreshToken.Token;
                RefreshTokenInDb.Expires = refreshToken.Expires;
                RefreshTokenInDb.UserId = refreshToken.UserId;
            }
        }

        
    }
}
