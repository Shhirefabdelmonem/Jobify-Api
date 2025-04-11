using Jobify.Core.Interfaces.IRepos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Core.Interfaces.IRepos
{
    public interface IUnitOfWork
    {
        public ITokenRepo Token { get; }
        Task<int> CompleteAsync();

    }
}
