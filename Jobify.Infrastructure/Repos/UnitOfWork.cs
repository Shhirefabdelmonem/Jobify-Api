using Jobify.Core.Interfaces.IRepos;
using Jobify.Core.Interfaces.IRepos.Authentication;
using Jobify.Infrastructure.Data;
using Jobify.Infrastructure.Repos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Infrastructure.Repos
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly ApplicationDbContext _dbContext;

        public ITokenRepo Token{ get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
            Token = new TokenRepo(_dbContext);
        }

        

        public async Task<int> CompleteAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }

        
    }
}
