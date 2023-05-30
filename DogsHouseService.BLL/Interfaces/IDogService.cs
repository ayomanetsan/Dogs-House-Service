using DogsHouseService.DAL.Entities;
using Microsoft.Data.SqlClient;

namespace DogsHouseService.BLL.Interfaces
{
    public interface IDogService
    {
        Task<IEnumerable<Dog>> GetAllDogsAsync();

        Task<IEnumerable<Dog>> GetSortedDogsAsync(string attribute, SortOrder order);

        Task<IEnumerable<Dog>> GetPagedDogsAsync(int pageNumber, int pageSize);

        Task<IEnumerable<Dog>> GetPagedAndSortedDogsAsync(int pageNumber, int pageSize, string sortBy, SortOrder sortOrder);
    }
}
