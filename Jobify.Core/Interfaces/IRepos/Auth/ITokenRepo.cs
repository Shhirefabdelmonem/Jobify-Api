using Jobify.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Core.Interfaces.IRepos.Authentication
{
    public interface ITokenRepo : IGenericRepo<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
    }
    
}
