using DogsHouseService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.DAL.Context
{
    public class DogsHouseServiceDbContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        public DogsHouseServiceDbContext(DbContextOptions<DogsHouseServiceDbContext> options)
        : base(options)
        {
        }
    }
}
