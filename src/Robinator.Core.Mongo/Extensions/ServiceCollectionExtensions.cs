using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Robinator.Core.Areas.Pages;
using Robinator.Core.Mongo.Pages;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRobinatorMongo(this IServiceCollection services, string mongoUri, string databaseName)
        {
            services.AddScoped<IMongoClient>(r => new MongoClient(mongoUri));
            services.AddScoped(r => r.GetService<IMongoClient>().GetDatabase(databaseName));
            services.AddTransient<IPageRepository, PageRepository>();
            BsonClassMap.RegisterClassMap<Page>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
            });
            return services;
        }
    }
}
