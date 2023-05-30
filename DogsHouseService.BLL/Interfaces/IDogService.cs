using DogsHouseService.DAL.Entities;

namespace DogsHouseService.BLL.Interfaces
{
    public interface IDogService
    {
        Task<IEnumerable<Dog>> GetAllDogsAsync();

        Task<IEnumerable<Dog>> GetSortedDogsAsync(string attribute, string order);

        Task<IEnumerable<Dog>> GetPagedDogsAsync(int pageNumber, int pageSize);

        Task<IEnumerable<Dog>> GetPagedAndSortedDogsAsync(int pageNumber, int pageSize, string attribute, string order);

        Task CreateDogAsync(Dog dog);
    }
}
