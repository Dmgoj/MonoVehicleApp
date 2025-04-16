using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Project.Common;

namespace Project.Repository.Common
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            PagingParameters pagingParameters = null);

        Task<TEntity> GetByID(object id);

        Task Insert(TEntity entity);

        Task Delete(object id);

        Task Delete(TEntity entityToDelete);

        Task Update(TEntity entityToUpdate);
    }
}
