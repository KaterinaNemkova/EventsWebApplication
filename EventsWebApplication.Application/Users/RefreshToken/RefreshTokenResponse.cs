using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Users.RefreshToken
{
    public class RefreshTokenResponse
    {
        public required string JwtToken { get; set; }

        public required string RefreshToken { get; set; }
    }
}
