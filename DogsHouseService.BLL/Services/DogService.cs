using AutoMapper;
using DogsHouseService.BLL.Helpers;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.Common.DTO.Dog;
using DogsHouseService.DAL.Context;
using DogsHouseService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.BLL.Services
{
    public class DogService : IDogService
    {
        private readonly DogsHouseServiceDbContext _context;
        private readonly IMapper _mapper;

        public DogService(DogsHouseServiceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DogDto>> GetAllDogsAsync()
        {
            var dogs = await _context.Dogs.ToListAsync();
            return _mapper.Map<IEnumerable<DogDto>>(dogs);
        }

        public async Task<IEnumerable<DogDto>> GetSortedDogsAsync(string attribute, string order)
        {
            var dogs = await DogHelperMethods.ApplySortByAttribute(_context.Dogs, attribute, order).ToListAsync();
            return _mapper.Map<IEnumerable<DogDto>>(dogs);
        }

        public async Task<IEnumerable<DogDto>> GetPagedDogsAsync(int pageNumber, int pageSize)
        {
            DogHelperMethods.ValidatePage(pageNumber, pageSize);

            int skipCount = (pageNumber - 1) * pageSize;

            var dogs = await _context.Dogs.Skip(skipCount).Take(pageSize).ToListAsync();
            return _mapper.Map<IEnumerable<DogDto>>(dogs);
        }

        public async Task<IEnumerable<DogDto>> GetPagedAndSortedDogsAsync(int pageNumber, int pageSize, string attribute, string order)
        {
            var query = DogHelperMethods.ApplySortByAttribute(_context.Dogs, attribute, order);

            DogHelperMethods.ValidatePage(pageNumber, pageSize);

            int skipCount = (pageNumber - 1) * pageSize;

            var dogs = await query.Skip(skipCount).Take(pageSize).ToListAsync();
            return _mapper.Map<IEnumerable<DogDto>>(dogs);
        }

        public async Task<DogDto> CreateDogAsync(DogDto newDog)
        {
            var dog = _mapper.Map<Dog>(newDog);
            DogHelperMethods.ValidateDog(dog);
            
            if (await _context.Dogs.AnyAsync(d => d.Name == dog.Name))
            {
                throw new InvalidOperationException("Dog already exists.");
            }

            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();

            return _mapper.Map<DogDto>(dog);
        }
    }
}
