using DogsHouseService.BLL.Helpers;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.DAL.Context;
using DogsHouseService.DAL.Entities;
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

        public async Task<IEnumerable<Dog>> GetSortedDogsAsync(string attribute, string order)
        {
            return await DogSortingHelper.ApplySortByAttribute(_context.Dogs, attribute, order).ToListAsync();
        }

        public async Task<IEnumerable<Dog>> GetPagedDogsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.");
            }
            else if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.");
            }    

            int skipCount = (pageNumber - 1) * pageSize;

            return await _context.Dogs.Skip(skipCount).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Dog>> GetPagedAndSortedDogsAsync(int pageNumber, int pageSize, string attribute, string order)
        {
            var query = DogSortingHelper.ApplySortByAttribute(_context.Dogs, attribute, order);

            int skipCount = (pageNumber - 1) * pageSize;

            return await _context.Dogs.Skip(skipCount).Take(pageSize).ToListAsync();
        }   

        public async Task CreateDogAsync(Dog dog)
        {
            if (await _context.Dogs.AnyAsync(d => d.Name == dog.Name))
            {
                throw new InvalidOperationException("Dog already exists.");
            }

            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();
        }
    }
}
