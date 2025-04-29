using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service;
using Project.Service.Common;
using Project.Service.Common.Parameters;
using Xunit;

namespace Project.Service.Tests
{
    public class VehicleModelServiceTests
    {
        private readonly Mock<IRepository<VehicleModel>> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly VehicleModelService _service;

        public VehicleModelServiceTests()
        {
            _repoMock = new Mock<IRepository<VehicleModel>>();
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new VehicleModelService(
                _repoMock.Object,
                _uowMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_NoFilter_Returns_PaginatedResult()
        {
            // Arrange
            var parameters = new QueryParameters { PageNumber = 1, PageSize = 2 };
            var entities = new List<VehicleModel>
            {
                new() { Id = 1, Name = "A" },
                new() { Id = 2, Name = "B" },
                new() { Id = 3, Name = "C" }
            };
            var paged = entities.Take(2).ToList();
            var dtos = paged.Select(e => new VehicleModelDto { Id = e.Id, Name = e.Name, Abrv = e.Abrv }).ToList();

            // Setup repository: full list for count
            _repoMock
                .Setup(r => r.Get(
                    It.Is<Expression<Func<VehicleModel, bool>>>(f => f == null),
                    null,
                    string.Empty,
                    null))
                .ReturnsAsync(entities);

            // Setup repository: paged list
            _repoMock
                .Setup(r => r.Get(
                    It.Is<Expression<Func<VehicleModel, bool>>>(f => f == null),
                    null,
                    string.Empty,
                    parameters))
                .ReturnsAsync(paged);

            // Setup mapper
            _mapperMock
                .Setup(m => m.Map<IEnumerable<VehicleModelDto>>(paged))
                .Returns(dtos);

            // Act
            var result = await _service.GetAllAsync(parameters);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEquivalentTo(dtos);
            result.TotalCount.Should().Be(3);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(2);
        }

        [Fact]
        public async Task GetByIdAsync_EntityExists_Returns_Dto()
        {
            // Arrange
            var entity = new VehicleModel { Id = 5, Name = "X" };
            var dto = new VehicleModelDto { Id = 5, Name = "X", Abrv = entity.Abrv };

            _repoMock.Setup(r => r.GetByID(5)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<VehicleModelDto>(entity)).Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(5);

            // Assert
            result.Should().Be(dto);
        }

        [Fact]
        public async Task GetByIdAsync_EntityMissing_Throws_KeyNotFoundException()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByID(42)).ReturnsAsync((VehicleModel)null);

            // Act
            Func<Task> act = () => _service.GetByIdAsync(42);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage("VehicleModel with ID 42 not found.");
        }

        [Fact]
        public async Task CreateAsync_Inserts_And_Returns_Dto()
        {
            // Arrange
            var createDto = new VehicleModelForCreateDto { Name = "New", Abrv = "NW" };
            var entity = new VehicleModel { Id = 7, Name = "New", Abrv = "NW" };
            var dto = new VehicleModelDto { Id = 7, Name = "New", Abrv = "NW" };

            _mapperMock.Setup(m => m.Map<VehicleModel>(createDto)).Returns(entity);
            _repoMock.Setup(r => r.Insert(entity)).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.Save()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<VehicleModelDto>(entity)).Returns(dto);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            result.Should().Be(dto);
            _repoMock.Verify(r => r.Insert(entity), Times.Once);
            _uowMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_EntityExists_Updates_And_Saves()
        {
            // Arrange
            var updateDto = new VehicleModelForUpdateDto { Name = "Upd" };
            var entity = new VehicleModel { Id = 9, Name = "Old" };

            _repoMock.Setup(r => r.GetByID(9)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map(updateDto, entity));
            _repoMock.Setup(r => r.Update(entity)).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.Save()).ReturnsAsync(1);

            // Act
            await _service.UpdateAsync(9, updateDto);

            // Assert
            _mapperMock.Verify(m => m.Map(updateDto, entity), Times.Once);
            _repoMock.Verify(r => r.Update(entity), Times.Once);
            _uowMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_EntityMissing_Throws_KeyNotFoundException()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByID(13)).ReturnsAsync((VehicleModel)null);
            var updateDto = new VehicleModelForUpdateDto();

            // Act
            Func<Task> act = () => _service.UpdateAsync(13, updateDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage("VehicleModel with ID 13 not found.");
        }

        [Fact]
        public async Task DeleteAsync_Calls_Delete_And_Save()
        {
            // Arrange
            _repoMock.Setup(r => r.Delete(11)).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.Save()).ReturnsAsync(1);

            // Act
            await _service.DeleteAsync(11);

            // Assert
            _repoMock.Verify(r => r.Delete(11), Times.Once);
            _uowMock.Verify(u => u.Save(), Times.Once);
        }
    }
}
