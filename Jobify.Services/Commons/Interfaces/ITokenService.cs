using Jobify.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Commons.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime Expires) GenerateJwtToken(AppUser user);
        Task<RefreshToken> GenerateRefreshTokenAsync(AppUser user);
    }
}
