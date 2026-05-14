using Domain.Products.ReadModels;
using MongoDB.Bson.Serialization;

namespace Consumer.Infrastructure.Persistence;

public static class MongoConfiguration
{
    public static void RegisterClassMaps()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(ProductReadModel)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<ProductReadModel>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id);
        });
    }
}
