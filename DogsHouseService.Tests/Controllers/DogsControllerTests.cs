using DogsHouseService.BLL.Interfaces;
using DogsHouseService.DAL.Entities;
using DogsHouseService.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DogsHouseService.Tests.Controllers
{
    public class DogsControllerTests
    {
        private readonly IDogService _dogsService;
        private readonly DogsController _sut;

        public DogsControllerTests()
        {
            _dogsService = A.Fake<IDogService>();
            _sut = new DogsController(_dogsService);
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

            A.CallTo(() => _dogsService.GetAllDogsAsync())
                .Returns(dogs);

            var result = await _sut.GetDogs();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(dogs, okObjectResult.Value);
        }
    }
}
