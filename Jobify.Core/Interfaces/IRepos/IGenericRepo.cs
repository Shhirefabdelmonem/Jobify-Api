using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Core.Interfaces.IRepos
{
    public interface IGenericRepo<T> where T : class
    {
        
        Task AddAsync(T entity);
         Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
