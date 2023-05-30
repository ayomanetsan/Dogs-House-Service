using DogsHouseService.BLL.Interfaces;
using DogsHouseService.DAL.Context;
using DogsHouseService.DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<IEnumerable<Dog>> GetSortedDogsAsync(string attribute, SortOrder order)
        {
            IQueryable<Dog> query = _context.Dogs;

            switch (attribute)
            {
                case "name":
                    query = SortBy(query, d => d.Name, order);
                    break;
                case "color":
                    query = SortBy(query, d => d.Color, order);
                    break;
                case "tail_length":
                    query = SortBy(query, d => d.Tail_Length, order);
                    break;
                case "weight":
                    query = SortBy(query, d => d.Weight, order);
                    break;
                default:
                    throw new ArgumentException("Invalid attribute value.");
            }

            return await query.ToListAsync();
        }

        private static IQueryable<T> SortBy<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, SortOrder order)
        {
            return order == SortOrder.Ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
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
