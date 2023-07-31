using MongoDB.Bson;
using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Otus.Teaching.Pcf.Administration.Mongo;

public class MongoDataContext
{
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly Dictionary<string, string> _collectionNames;

    public MongoDataContext(string connectionString)
    {
        _mongoClient = new MongoClient(connectionString);
        _mongoDatabase = _mongoClient.GetDatabase("root");
        _collectionNames = new Dictionary<string, string>();

        RegisterCollection<Role>(nameof(Roles));
        RegisterCollection<Employee>(nameof(Employees), "roleId");
    }
    public IMongoCollection<Role> Roles => GetCollections<Role>();

    public IMongoCollection<Employee> Employees => GetCollections<Employee>();
    public void RegisterCollection<TDocument>(string collectionName, params string[] indexProperties)
    {
        if (_collectionNames.ContainsKey(typeof(TDocument).Name)) return;

        _collectionNames.Add(typeof(TDocument).Name, collectionName);

        if (indexProperties?.Any() == true)
        {
            var collection = GetCollections<TDocument>(collectionName);

            Array.ForEach(indexProperties, index =>
            {
                CreateIndex(collection, index);
            });
        }
    }

    public bool HasCollection<TDocument>()
    {
        return _collectionNames.ContainsKey(typeof(TDocument).Name);
    }

    public string GetCollectionName<TDocument>()
    {
        if (!_collectionNames.ContainsKey(typeof(TDocument).Name))
            throw new InvalidOperationException(
                $"The class {typeof(TDocument).Name} does not have BsonCollectionAttribute attribute");

        return _collectionNames[typeof(TDocument).Name];
    }

    public void DropCollection<TDocument>()
    {
        if (!_collectionNames.ContainsKey(typeof(TDocument).Name))
            throw new InvalidOperationException(
                $"The class {typeof(TDocument).Name} does not have BsonCollectionAttribute attribute");

        var collectionName = _collectionNames[typeof(TDocument).Name];

        _mongoDatabase.DropCollection(collectionName);
    }

    public IMongoCollection<TDocument> GetCollections<TDocument>()
    {
        if (!_collectionNames.ContainsKey(typeof(TDocument).Name))
            throw new InvalidOperationException(
                $"The class {typeof(TDocument).Name} does not have BsonCollectionAttribute attribute");

        var collectionName = _collectionNames[typeof(TDocument).Name];

        return GetCollections<TDocument>(collectionName);
    }

    private IMongoCollection<TDocument> GetCollections<TDocument>(string collectionName)
        => _mongoDatabase.GetCollection<TDocument>(collectionName);

    public Task<long> CountAsync<TDocument>(IMongoCollection<TDocument> collection)
    {
        return collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
    }

    public long Count<TDocument>(IMongoCollection<TDocument> collection)
    {
        return collection.CountDocuments(FilterDefinition<TDocument>.Empty);
    }

    public Task AddAsync<TDocument>(IMongoCollection<TDocument> collection, TDocument document)
    {
        return collection.InsertOneAsync(document);
    }

    public Task AddRangeAsync<TDocument>(IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents)
    {
        return collection.InsertManyAsync(documents);
    }

    public Task UpdateAsync<TDocument>(IMongoCollection<TDocument> collection, TDocument document)
    {
        return collection.ReplaceOneAsync(
            GetDocumentFilter(document),
            document,
            new ReplaceOptions
            {
                IsUpsert = true
            });
    }

    public async Task UpdateRangeAsync<TDocument>(IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents)
    {
        var tasks = documents.Select(document => UpdateAsync(collection, document));

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    public Task RemoveAsync<TDocument>(IMongoCollection<TDocument> collection, TDocument document)
    {
        return collection.DeleteOneAsync(GetDocumentFilter(document));
    }

    public Task RemoveRangeAsync<TDocument>(IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents)
    {
        return collection.DeleteManyAsync(GetDocumentFilter(documents));
    }

    public async Task<TDocument> GetByIdAsync<TDocument>(IMongoCollection<TDocument> collection, string id)
    {
        var res = await collection.FindAsync(GetDocumentFilter(id)).ConfigureAwait(false);
        return await res.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<TDocument>> GetAllAsync<TDocument>(IMongoCollection<TDocument> collection)
    {
        var res = await collection.FindAsync(FilterDefinition<TDocument>.Empty).ConfigureAwait(false);
        var list = await res.ToListAsync().ConfigureAwait(false);
        return list;
    }

    public async Task<TDocument> GetFirstWhereAsync<TDocument>(IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> predicate)
    {
        var res = await collection.FindAsync(predicate).ConfigureAwait(false);
        return await res.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<TDocument>> GetRangeByIdsAsync<TDocument>(IMongoCollection<TDocument> collection, string[] ids)
    {
        var res = await collection.FindAsync(GetDocumentFilter(ids)).ConfigureAwait(false);
        var list = await res.ToListAsync().ConfigureAwait(false);
        return list;
    }

    public async Task<IEnumerable<TDocument>> GetWhereAsync<TDocument>(IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> predicate)
    {
        var res = await collection.FindAsync<TDocument>(predicate).ConfigureAwait(false);
        var list = await res.ToListAsync().ConfigureAwait(false);
        return list;
    }

    public bool HasIndex<TDocument>(IMongoCollection<TDocument> collection, string indexName)
    {
        var indexes = collection.Indexes.List().ToList();
        var indexNames = indexes
            .SelectMany(i => i.Elements)
            .Where(e => string.Equals(e.Name, "name", StringComparison.CurrentCultureIgnoreCase))
            .Select(n => n.Value.ToString())
            .ToList();

        return indexNames.Contains(indexName);
    }

    public async Task CreateIndexAsync<TDocument>(IMongoCollection<TDocument> collection, string field, bool isUnique = false)
    {
        var keysBuilder = Builders<TDocument>.IndexKeys.Ascending(field);
        if (HasIndex(collection, field))
        {
            return;
        }

        var indexOptions = new CreateIndexOptions
        {
            Name = field,
            Unique = isUnique
        };

        var indexModel = new CreateIndexModel<TDocument>(keysBuilder, indexOptions);

        await collection.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false);
    }

    public void CreateIndex<TDocument>(IMongoCollection<TDocument> collection, string field, bool isUnique = false)
    {
        var keysBuilder = Builders<TDocument>.IndexKeys.Ascending(field);
        if (HasIndex(collection, field))
        {
            return;
        }

        var indexOptions = new CreateIndexOptions
        {
            Name = field,
            Unique = isUnique
        };

        var indexModel = new CreateIndexModel<TDocument>(keysBuilder, indexOptions);

        collection.Indexes.CreateOne(indexModel);
    }

    public async Task DropIndexAsync<TDocument>(IMongoCollection<TDocument> collection, string field)
    {
        if (!HasIndex(collection, field))
        {
            return;
        }
        await collection.Indexes.DropOneAsync(field).ConfigureAwait(false);
    }

    public void DropIndex<TDocument>(IMongoCollection<TDocument> collection, string field)
    {
        if (!HasIndex(collection, field))
        {
            return;
        }

        collection.Indexes.DropOne(field);
    }

    private static BsonDocument GetDocumentFilter<TDocument>(IEnumerable<TDocument> documents)
    {
        return new BsonDocument("$or", new BsonArray(documents.Select(GetDocumentFilter)));
    }

    private static BsonDocument GetDocumentFilter<TDocument>(TDocument document)
    {
        return new BsonDocument("_id", new BsonDocument("$eq", document.ToBsonDocument()["_id"]));
    }

    private static BsonDocument GetDocumentFilter(string[] ids)
    {
        return new BsonDocument("$or", new BsonArray(ids.Select(GetDocumentFilter)));
    }

    private static BsonDocument GetDocumentFilter(string id)
    {
        return new BsonDocument("_id", new BsonDocument("$eq", id));
    }
}
