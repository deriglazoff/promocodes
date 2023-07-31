using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Otus.Teaching.Pcf.Administration.Mongo.Repositories;

public class MongoRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly MongoDataContext _mongoDataContext;
    private readonly IMongoCollection<TEntity> _mongoCollection;

    protected MongoRepository(MongoDataContext mongoDataContext)
    {
        _mongoDataContext = mongoDataContext;
        _mongoCollection = _mongoDataContext.GetCollections<TEntity>();
    }

    public Task AddAsync(TEntity entity)
    {
        return _mongoCollection.InsertOneAsync(entity);
    }
    public Task UpdateAsync(TEntity entity)
    {
        return _mongoCollection.ReplaceOneAsync(item => item.Id == entity.Id, entity,
            new ReplaceOptions
            {
                IsUpsert = true
            });
    }

    public Task DeleteAsync(TEntity entity)
    {
        return _mongoCollection.DeleteOneAsync(item => item.Id == entity.Id);
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return _mongoDataContext.GetAllAsync(_mongoCollection);
    }

    public Task<TEntity> GetByIdAsync(Guid id)
    {
        return _mongoDataContext.GetByIdAsync(_mongoCollection, id.ToString());
    }

    public Task<TEntity> GetFirstWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return _mongoDataContext.GetFirstWhereAsync(_mongoCollection, predicate);
    }

    public Task<IEnumerable<TEntity>> GetRangeByIdsAsync(List<Guid> ids)
    {
        return _mongoDataContext.GetRangeByIdsAsync(_mongoCollection, ids.Select(x => x.ToString()).ToArray());
    }

    public Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return _mongoDataContext.GetWhereAsync(_mongoCollection, predicate);
    }
}
