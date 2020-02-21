using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CFBS.Common.Repository
{
    public interface IGenericRepository<TEntity, TDTO> where TEntity : class 
                                                       where TDTO : class
    {
        Task<ICollection<TDTO>> Get(Expression<Func<TEntity, bool>> filter = null,
                                    string includeProperties = "");
        Task<TDTO> GetByID(int id);
        Task<TDTO> Create(TDTO dto);
        Task Delete(int id);
        Task Update(int id, TDTO dto);
        Task<bool> EntityExists(int id);
    }
}