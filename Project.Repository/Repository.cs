using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Common;
using Project.Repository.Common;

namespace Project.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly bool _isReadOnly; // Used for VehicleEngineType read only requirement

        public Repository(DbContext context, bool isReadOnly = false)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _isReadOnly = isReadOnly;
        }

        public async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            PagingParameters pagingParameters = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            if (pagingParameters != null)
            {
                query = query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                             .Take(pagingParameters.PageSize);
            }
            
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByID(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Insert(TEntity entity)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("This repository is read-only.");
            }
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(object id)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("This repository is read-only.");
            }
            var entityToDelete = _dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }

        public Task Delete(TEntity entityToDelete)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("This repository is read-only.");
            }
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public Task Update(TEntity entityToUpdate)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("This repository is read-only.");
            }
            _dbSet.Attach(entityToUpdate);
             _context.Entry(entityToUpdate).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
