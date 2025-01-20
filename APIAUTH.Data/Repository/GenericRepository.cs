using APIAUTH.Data.Context;
using APIAUTH.Domain.Entities;
using APIAUTH.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Data.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AuthContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(AuthContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> Add(TEntity item)
        {
            _dbSet.Add(item);
            await Save();
            return item;
        }

        public async Task<TEntity> Update(TEntity item)
        {
            var existingEntity = _context.ChangeTracker.Entries<TEntity>()
            .FirstOrDefault(e => e.Entity.Id == item.Id)?.Entity;

            if (existingEntity != null)
            {
                // Detach la entidad existente para evitar conflictos
                _context.Entry(existingEntity).State = EntityState.Detached;
            }
            // Adjunta la entidad y marca su estado como modificado
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;

            await Save();
            return item;
        }

        public async Task<TEntity> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.Where(filter);
        }

        //TODO: Validar ya que es una sql query
        public async Task<IEnumerable<T>> ExecuteQuery<T>(string sqlQuery, params object[] parameters)
        {
            return _context.Database.SqlQueryRaw<T>(sqlQuery, parameters);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
