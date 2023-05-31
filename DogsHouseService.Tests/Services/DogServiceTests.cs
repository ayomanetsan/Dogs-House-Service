using DogsHouseService.BLL.Helpers;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.BLL.Services;
using DogsHouseService.DAL.Context;
using DogsHouseService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DogsHouseService.Tests.Services
{
    public class DogServiceTests
    {
        private readonly DogsHouseServiceDbContext _context;
        private readonly IDogService _sut;

        public DogServiceTests()
        {
            var options = new DbContextOptionsBuilder<DogsHouseServiceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DogsHouseServiceDbContext(options);
            _sut = new DogService(_context);
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

            Assert.Equal(dogs, result);
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

            Assert.Equal(sortedDogs, result);
        }

        [Theory]
        [InlineData("name", " ")]
        [InlineData("breed", "asc")]
        public async Task GetSortedDogsAsync_WhenInvalidParameters_ThrowsArgumentException(string attribute, string order)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.GetSortedDogsAsync(attribute, order));
        }
    }
}
