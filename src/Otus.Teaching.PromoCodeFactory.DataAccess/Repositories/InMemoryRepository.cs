using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        protected List<T> Data { get; set; }

        public InMemoryRepository(List<T> data)
        {
            Data = data;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.AsEnumerable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<T> Create(T entity)
        {
            Data.Add(entity);
            return Task.FromResult(entity);
        }

        public Task RemoveByIdAsync(Guid id)
        {
            Data.Remove(Data.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public Task<T> UpdateByIdAsync(T entity)
        {
            RemoveByIdAsync(entity.Id);
            Create(entity);
            return Task.FromResult(entity);
        }
    }
}