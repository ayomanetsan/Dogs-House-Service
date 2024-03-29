﻿using AutoMapper;
using DogsHouseService.BLL.Helpers;
using DogsHouseService.BLL.Interfaces;
using DogsHouseService.BLL.MappingProfiles;
using DogsHouseService.Common.DTO.Dog;
using DogsHouseService.DAL.Entities;
using DogsHouseService.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DogsHouseService.Tests.Controllers
{
    public class DogsControllerTests
    {
        private readonly IDogService _dogService;
        private readonly IMapper _mapper;
        private readonly DogsController _sut;

        public DogsControllerTests()
        {
            _dogService = A.Fake<IDogService>();
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<DogProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();
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
        public async Task GetDogs_WhenNoParameters_ReturnsOkWithAllDogs()
        {
            var dogs = new List<DogDto>()
            {
                new DogDto { Name = "John", Color = "White", Tail_Length = 1, Weight = 1 },
                new DogDto { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 2 }
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

            var sortedDogs = _mapper.Map<IEnumerable<DogDto>>(DogHelperMethods.ApplySortByAttribute(dogs.AsQueryable(), attribute, order).ToList());
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

            IActionResult result;
            try
            {
                result = await _sut.GetDogs(attribute: attribute, order: order);
            }
            catch (Exception ex)
            {
                var request = new BadRequestObjectResult(ex.Message);
                request.StatusCode = (int)HttpStatusCode.BadRequest;
                result = request;
            }

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(3, 10)]
        public async Task GetDogs_WhenValidPageNumberAndPageSize_ReturnsOkPagedDogs(int pageNumber, int pageSize)
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };

            var pagedDogs =  _mapper.Map<IEnumerable<DogDto>>(dogs.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize));
            A.CallTo(() => _dogService.GetPagedDogsAsync(pageNumber, pageSize))
                .Returns(pagedDogs);

            var result = await _sut.GetDogs(pageNumber, pageSize);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(pagedDogs, okObjectResult.Value);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        public async Task GetDogs_WhenInvalidPageNumberAndPageSize_ReturnsBadRequest(int pageNumber, int pageSize)
        {
            A.CallTo(() => _dogService.GetPagedDogsAsync(pageNumber, pageSize))
                .Throws<ArgumentException>();

            IActionResult result;
            try
            {
                result = await _sut.GetDogs(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                var request = new BadRequestObjectResult(ex.Message);
                request.StatusCode = (int)HttpStatusCode.BadRequest;
                result = request;
            }

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Theory]
        [InlineData(1, 2, "name", "desc")]
        [InlineData(2, 1, "color", "asc")]
        [InlineData(3, 10, "weight", "asc")]
        public async Task GetDogs_WhenValidParameters_ReturnsOkPagedAndSortedDogs(int pageNumber, int pageSize, string attribute, string order)
        {
            var dogs = new List<Dog>()
            {
                new Dog { Name = "John", Color = "White", Tail_Length = 1, Weight = 10 },
                new Dog { Name = "Jane", Color = "Black", Tail_Length = 2, Weight = 5 },
                new Dog { Name = "Max", Color = "Brown", Tail_Length = 3, Weight = 8 },
            };

            var sortedDogs = DogHelperMethods.ApplySortByAttribute(dogs.AsQueryable(), attribute, order).ToList();
            var pagedAndSortedDogs = _mapper.Map<IEnumerable<DogDto>>(sortedDogs.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize));
            A.CallTo(() => _dogService.GetPagedAndSortedDogsAsync(pageNumber, pageSize, attribute, order))
                .Returns(pagedAndSortedDogs);

            var result = await _sut.GetDogs(pageNumber, pageSize, attribute, order);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(pagedAndSortedDogs, okObjectResult.Value);
        }

        [Theory]
        [InlineData(-1, 10, "name", "desc")]
        [InlineData(5, 1, "breed", "asc")]
        [InlineData(3, -1, " ", "crusty")]
        public async Task GetDogs_WhenInvalidParameters_ReturnsBadRequest(int pageNumber, int pageSize, string attribute, string order)
        {
            A.CallTo(() => _dogService.GetPagedAndSortedDogsAsync(pageNumber, pageSize, attribute, order))
                .Throws<ArgumentException>();

            IActionResult result;
            try
            {
                result = await _sut.GetDogs(pageNumber, pageSize, attribute, order);
            }
            catch (Exception ex)
            {
                var request = new BadRequestObjectResult(ex.Message);
                request.StatusCode = (int)HttpStatusCode.BadRequest;
                result = request;
            }

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateDog_WhenValidNonExistingDog_ReturnsCreated()
        {
            var dog = new DogDto
            {
                Name = "John",
                Color = "White",
                Tail_Length = 1,
                Weight = 1
            };

            var result = await _sut.CreateDog(dog);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateDog_WhenValidExistingDog_ReturnsBadRequest()
        {
            var dog = new DogDto
            {
                Name = "John",
                Color = "White",
                Tail_Length = 1,
                Weight = 1
            };

            await _sut.CreateDog(dog);
            A.CallTo(() => _dogService.CreateDogAsync(dog))
                .Throws<InvalidOperationException>();

            IActionResult result;
            try
            {
                result = await _sut.CreateDog(dog);
            }
            catch (Exception ex)
            {
                var request = new BadRequestObjectResult(ex.Message);
                request.StatusCode = (int)HttpStatusCode.BadRequest;
                result = request;
            }

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateDog_WhenInvalidModel_ReturnsBadRequest()
        {
            var dog = new DogDto
            {
                Name = "John",
                Color = "White",
                Tail_Length = 1,
                Weight = 1
            };
            _sut.ModelState.AddModelError("Invalid", "Fake model state");

            var result = await _sut.CreateDog(dog);

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }
    }
}
