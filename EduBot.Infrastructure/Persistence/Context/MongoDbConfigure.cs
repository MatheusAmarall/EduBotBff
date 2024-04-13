using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace EduBot.Infrastructure.Persistence.Context;

public static class MongoDbConfigure {
    public static void Configure() {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfDefaultConvention(true)
        };
        ConventionRegistry.Register("My Solution Conventions", pack, _ => true);
        BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
    }
}