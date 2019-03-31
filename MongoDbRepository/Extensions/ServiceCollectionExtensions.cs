using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbRepository.Interfaces;
using MongoDbRepository.Repository;

namespace MongoDbRepository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterMongoDbRepository(this IServiceCollection collection, IConfiguration Configuration, string password = null)
        {
            var connection = Configuration.GetConnectionString("MongoDb");

            if(!string.IsNullOrWhiteSpace(password))
            {
                connection = connection.Replace("<password>", password);
            }

            collection.AddSingleton<IUnitOfWork>(new UnitOfWork(connection));
            collection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }
    }
}
