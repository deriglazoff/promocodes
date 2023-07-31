using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess.Data;

namespace Otus.Teaching.Pcf.Administration.Mongo.Data;

public class MongoDbInitializer : IDbInitializer
{
    private readonly MongoDataContext _mongoDataContext;

    public MongoDbInitializer(MongoDataContext mongoDataContext)
    {
        _mongoDataContext = mongoDataContext;
    }

    public void InitializeDb()
    {
        _mongoDataContext.DropCollection<Role>();

        if (_mongoDataContext.Count(_mongoDataContext.Roles) == 0)
        {
            _mongoDataContext.AddRangeAsync(_mongoDataContext.Roles, FakeDataFactory.Roles);
        }

        _mongoDataContext.DropCollection<Employee>();

        if (_mongoDataContext.Count(_mongoDataContext.Employees) == 0)
        {
            _mongoDataContext.AddRangeAsync(_mongoDataContext.Employees, FakeDataFactory.Employees);
        }
    }
}
