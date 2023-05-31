using DogsHouseService.BLL.Helpers;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.BLL.Services;
using DogsHouseService.DAL.Entities;
using DogsHouseService.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DogsHouseService.Tests.Controllers
{
    public class DogsControllerTests
    {
        private readonly IDogService _dogService;
        private readonly DogsController _sut;

        public DogsControllerTests()
        {
            _dogService = A.Fake<IDogService>();
            _sut = new DogsController(_dogService);
        }

        [Fact]
        public void GetApiVersion_ReturnsOk()
        {
            var result = _sut.GetApiVersion();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetDogs_WhenNoParameteres_ReturnsOkWithAllDogs()
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 1 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 2 }
            };

            A.CallTo(() => _dogService.GetAllDogsAsync())
                .Returns(dogs);

            var result = await _sut.GetDogs();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(dogs, okObjectResult.Value);
        }

        [Theory]
        [InlineData("name", "desc")]
        [InlineData("color", "asc")]
        [InlineData("weight", "asc")]      
        [InlineData("tail_length", "desc")]
        public async Task GetDogs_WhenValidAttributeAndOrder_ReturnsOkSortedDogs(string attribute, string order)
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };

            var sortedDogs = DogHelperMethods.ApplySortByAttribute(dogs.AsQueryable(), attribute, order).ToList();
            A.CallTo(() => _dogService.GetSortedDogsAsync(attribute, order))
                .Returns(sortedDogs);

            var result = await _sut.GetDogs(attribute: attribute, order: order);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(sortedDogs, okObjectResult.Value);
        }

        [Theory]
        [InlineData("name", " ")]
        [InlineData("breed", "asc")]
        public async Task GetDogs_WhenInvalidAttributeAndOrder_ReturnsBadRequest(string attribute, string order)
        {
            A.CallTo(() => _dogService.GetSortedDogsAsync(attribute, order))
                .Throws<ArgumentException>();

            var result = await _sut.GetDogs(attribute: attribute, order: order);

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }
    }
}
