using DogsHouseService.BLL.Interfaces;
using DogsHouseService.DAL.Context;
using DogsHouseService.DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.BLL.Services
{
    public class DogService : IDogService
    {
        private readonly DogsHouseServiceDbContext _context;

        public DogService(DogsHouseServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dog>> GetAllDogsAsync()
        {
            return await _context.Dogs.ToListAsync();
        }

        public Task<IEnumerable<Dog>> GetSortedDogsAsync(string sortBy, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dog>> GetPagedDogsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dog>> GetPagedAndSortedDogsAsync(int pageNumber, int pageSize, string sortBy, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }   
    }
}
