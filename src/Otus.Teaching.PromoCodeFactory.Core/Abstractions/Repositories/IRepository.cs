using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        
        public Task<T> GetByIdAsync(Guid id);

        public Task<T> Create(T entity);

        public Task RemoveByIdAsync(Guid id);

        public Task<T> UpdateByIdAsync(T entity);
    }
}