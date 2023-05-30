using DogsHouseService.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDogsHouseServiceDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionsString = configuration.GetConnectionString("DogsHouseServiceDbConnection");
            services.AddDbContext<DogsHouseServiceDbContext>(options =>
                options.UseSqlServer(
                    connectionsString,
                    opt => opt.MigrationsAssembly(typeof(DogsHouseServiceDbContext).Assembly.GetName().Name)));
        }
    }
}
