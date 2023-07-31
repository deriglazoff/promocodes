using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using MongoDB.Bson.Serialization.Conventions;

namespace Otus.Teaching.Pcf.Administration.Mongo;

public static class MongoServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMongoService(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        RegisterClassMaps();

        return services;
    }

    private static void RegisterClassMaps()
    {

        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
        };

        ConventionRegistry.Register("CamelCase", pack, t => true);

        BsonClassMap.RegisterClassMap<Role>(map =>
        {
            map.AutoMap();
        });

        BsonClassMap.RegisterClassMap<Employee>(map =>
        {
            map.AutoMap();

            map.MapMember(emp => emp.RoleId)
               .SetSerializer(new GuidSerializer(BsonType.String));


        });
    }
}
