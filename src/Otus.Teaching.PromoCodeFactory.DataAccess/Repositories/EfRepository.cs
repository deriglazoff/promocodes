using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<T> _entitySet;
        public EfRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _entitySet=_dataContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _entitySet.ToListAsync();

            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _entitySet.FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            var entities = await _entitySet.Where(x => ids.Contains(x.Id)).ToListAsync();
            return entities;
        }

        public async Task<T> Create(T entity)
        {
            await _entitySet.AddAsync(entity);

            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateByIdAsync(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var record = _entitySet.Find(id);
            _entitySet.Remove(record);
            await _dataContext.SaveChangesAsync();
        }


    }
}
