using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class ResponseDto
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiration { get; set; }
    }
}
