using Jobify.Core.Interfaces.IRepos;
using Jobify.Core.Interfaces.IRepos.Authentication;
using Jobify.Core.Models;
using Jobify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Infrastructure.Repos.Auth
{
    public class TokenRepo:GenericRepo<RefreshToken>, ITokenRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public TokenRepo(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }
    }
}
