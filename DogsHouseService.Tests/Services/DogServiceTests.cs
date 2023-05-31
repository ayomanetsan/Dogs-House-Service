using AutoMapper;
using DogsHouseService.BLL.Helpers;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.BLL.MappingProfiles;
using DogsHouseService.BLL.Services;
using DogsHouseService.Common.DTO.Dog;
using DogsHouseService.DAL.Context;
using DogsHouseService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DogsHouseService.Tests.Services
{
    public class DogServiceTests
    {    
        private readonly DogsHouseServiceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDogService _sut;

        public DogServiceTests()
        {
            var options = new DbContextOptionsBuilder<DogsHouseServiceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DogsHouseServiceDbContext(options);
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<DogProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();
            _sut = new DogService(_context, _mapper);
        }

        [Fact]
        public async Task GetAllDogsAsync_ReturnsAllDogs()
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };

            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            var result = await _sut.GetAllDogsAsync();

            Assert.Equal(dogs.Count(), result.Count());
        }

        [Theory]
        [InlineData("name", "desc")]
        [InlineData("color", "asc")]
        [InlineData("weight", "asc")]
        [InlineData("tail_length", "desc")]
        public async Task GetSortedDogsAsync_WhenValidParameters_ReturnsSortedDogs(string attribute, string order)
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };
            var sortedDogs = DogHelperMethods.ApplySortByAttribute(dogs.AsQueryable(), attribute, order).ToList();

            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            var result = await _sut.GetSortedDogsAsync(attribute, order);

            Assert.Equal(sortedDogs.Count(), result.Count());
        }

        [Theory]
        [InlineData("name", " ")]
        [InlineData("breed", "asc")]
        public async Task GetSortedDogsAsync_WhenInvalidParameters_ThrowsArgumentException(string attribute, string order)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.GetSortedDogsAsync(attribute, order));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(3, 10)]
        public async Task GetPagedDogsAsync_WhenValidParameters_ReturnsPagedDogs(int pageNumber, int pageSize)
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };
            var pagedDogs = dogs.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize);

            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            var result = await _sut.GetPagedDogsAsync(pageNumber, pageSize);

            Assert.Equal(pagedDogs.Count(), result.Count());
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(10, -1)]
        [InlineData(0, 0)]
        public async Task GetPagedDogsAsync_WhenInvalidParameters_ThrowsArgumentException(int pageNumber, int pageSize)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.GetPagedDogsAsync(pageNumber, pageSize));
        }

        [Theory]
        [InlineData(1, 2, "name", "desc")]
        [InlineData(2, 1, "color", "asc")]
        [InlineData(3, 10, "weight", "asc")]
        public async Task GetPagedAndSortedDogsAsync_WhenValidParameters_ReturnsPagedAndSortedDogs(int pageNumber, int pageSize, string attribute, string order)
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };
            var sortedDogs = DogHelperMethods.ApplySortByAttribute(dogs.AsQueryable(), attribute, order).ToList();
            var pagedAndSortedDogs = sortedDogs.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize);

            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            var result = await _sut.GetPagedAndSortedDogsAsync(pageNumber, pageSize, attribute, order);

            Assert.Equal(pagedAndSortedDogs.Count(), result.Count());
        }

        [Theory]
        [InlineData(-1, 10, "name", "desc")]
        [InlineData(5, 1, "breed", "asc")]
        [InlineData(3, -1, " ", "crusty")]
        public async Task GetPagedAndSortedDogsAsync_WhenInvalidParameters_ThrowsArgumentException(int pageNumber, int pageSize, string attribute, string order)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.GetPagedAndSortedDogsAsync(pageNumber, pageSize, attribute, order));
        }

        [Fact]
        public async Task CreateDog_WhenNonExistingDog_ReturnsCreatedDog()
        {
            var dog = new DogDto
            {
                Name = "John",
                Color = "White",
                Tail_Length = 1,
                Weight = 1,
            };

            var result = await _sut.CreateDogAsync(dog);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateDog_WhenExistingDog_ThrowsInvalidOperationException()
        {
            var dog = new DogDto
            {
                Name = "John",
                Color = "White",
                Tail_Length = 1,
                Weight = 1,
            };

            _context.Dogs.Add(_mapper.Map<Dog>(dog));
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.CreateDogAsync(dog));
        }
    }
}
