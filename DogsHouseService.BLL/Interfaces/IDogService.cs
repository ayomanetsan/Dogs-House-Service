using DogsHouseService.Common.DTO.Dog;

namespace DogsHouseService.BLL.Interfaces
{
    public interface IDogService
    {
        Task<IEnumerable<DogDto>> GetAllDogsAsync();

        Task<IEnumerable<DogDto>> GetSortedDogsAsync(string attribute, string order);

        Task<IEnumerable<DogDto>> GetPagedDogsAsync(int pageNumber, int pageSize);

        Task<IEnumerable<DogDto>> GetPagedAndSortedDogsAsync(int pageNumber, int pageSize, string attribute, string order);

        Task<DogDto> CreateDogAsync(DogDto dog);
    }
}
