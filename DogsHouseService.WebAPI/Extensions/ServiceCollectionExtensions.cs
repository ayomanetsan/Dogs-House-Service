using AspNetCoreRateLimit;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.BLL.Services;
using DogsHouseService.DAL.Context;
using Microsoft.EntityFrameworkCore;
using DogsHouseService.BLL.MappingProfiles;

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

        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IDogService, DogService>();
            services.AddAutoMapper(typeof(DogProfile).Assembly);
        }

        public static void AddIpRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }
    }
}
