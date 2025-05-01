using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.DAL.Entities;
using Moq;
using FluentAssertions;
using Project.Repository.Common;
using System.Linq.Expressions;
using Project.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq.EntityFrameworkCore;

namespace Project.Repository.Tests
{
    public class RepositoryTests
    {
        private readonly Mock<ProjectDbContext> _mockDbContext;
        private readonly List<VehicleMake> _vehicleMakes;
        private readonly IRepository<VehicleMake> _repository;

        public RepositoryTests()
        {
            _vehicleMakes = new List<VehicleMake>
            {
            new VehicleMake { Id = 1, Name = "Toyota", Abrv = "TYT" },
            new VehicleMake { Id = 2, Name = "Ford", Abrv = "FRD" },
            new VehicleMake { Id = 3, Name = "Honda", Abrv = "HND" },
            new VehicleMake { Id = 4, Name = "BMW", Abrv = "BMW" },
            new VehicleMake { Id = 5, Name = "Audi", Abrv = "AUD" }
            };

            _mockDbContext = new Mock<ProjectDbContext>();
            _mockDbContext
                .Setup(ctx => ctx.Set<VehicleMake>())
                .ReturnsDbSet(_vehicleMakes);

            // Setup FindAsync() to return the matching VehicleMake by Id
            _mockDbContext
                .Setup(ctx => ctx.Set<VehicleMake>()
                                .FindAsync(It.IsAny<object[]>())
                )
                .Returns<object[]>(keys =>
                {
                var id = (int)keys[0];
                var found = _vehicleMakes.SingleOrDefault(m => m.Id == id);
                return new ValueTask<VehicleMake?>(found);
                });

            _repository = new Repository<VehicleMake>(_mockDbContext.Object);
        }

        [Fact]
        public async Task Get_NoFilter_ReturnsAllMakes()
        {
            // Act
            var results = (await _repository.Get()).ToList();

            // Assert
            results.Should().HaveCount(5);
            results.Should().Contain(m => m.Name == "Ford");
        }

        [Fact]
        public async Task Get_WithFilter_ReturnsMatchingMakes()
        {
            // Act
            var results = (await _repository.Get(filter: m => m.Name.StartsWith("B"))).ToList();

            // Assert
            results.Should().HaveCount(1);
            results.Should().OnlyContain(m => m.Name.StartsWith("B"));
            results.Should().Contain(m => m.Abrv == "BMW");
            results.Should().NotContain(m => m.Abrv == "FRD");
        }

        [Fact]
        public async Task Get_WithOrderBy_ReturnsOrderedMakes()
        {
            // Act
            var results = (await _repository.Get(orderBy: q => q.OrderByDescending(m => m.Name))).ToList();

            // Assert
            var expectedOrder = new[] { "Toyota", "Honda", "Ford", "BMW", "Audi" };
            results.Select(m => m.Name).Should().Equal(expectedOrder);
        }

        [Fact]
        public async Task Get_WithPaging_ReturnsPagedMakes()
        {
            // Arrange
            var paging = new PagingParameters { PageNumber = 2, PageSize = 2 };

            // Act
            var results = (await _repository.Get(pagingParameters: paging)).ToList();

            // Assert
            results.Should().HaveCount(2);
            results[0].Name.Should().Be("Honda");
            results[1].Name.Should().Be("BMW");
        }

        [Fact]
        public async Task GetByID_ReturnsCorrectMake()
        {
            // Act
            var make = await _repository.GetByID(3);

            // Assert
            make.Should().NotBeNull();
            make.Name.Should().Be("Honda");
            _mockDbContext.Verify(c => c.Set<VehicleMake>().FindAsync(new object[] { 3 }), Times.Once);
        }

        [Fact]
        public async Task Insert_AddsNewMake()
        {
            // Arrange
            var newMake = new VehicleMake { Id = 6, Name = "Kia", Abrv = "KIA" };

            // Act
            await _repository.Insert(newMake);

            // Assert
            _mockDbContext.Verify(c =>
                c.Set<VehicleMake>().AddAsync(newMake, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Delete_ById_RemovesMake()
        {
            // Act
            await _repository.Delete(2);

            // Assert
            _mockDbContext.Verify(c => c.Set<VehicleMake>().FindAsync(new object[] { 2 }), Times.Once);
            _mockDbContext.Verify(c => c.Set<VehicleMake>().Remove(
                It.Is<VehicleMake>(m => m.Id == 2)), Times.Once);
        }

        [Fact]
        public async Task Update_CallsDbSetUpdate()
        {
            // Arrange
            var updated = new VehicleMake { Id = 5, Name = "Audi Updated", Abrv = "AUD" };
          
            // Act
            await _repository.Update(updated);

            // Assert
            _mockDbContext.Verify(c => c.Set<VehicleMake>().Update(updated), Times.Once);
           
        }

        [Fact]
        public async Task ReadOnlyRepository_ThrowsOnMutations()
        {
            // Arrange
            var readOnlyRepo = new Repository<VehicleMake>(_mockDbContext.Object, isReadOnly: true);

            // Act
            Func<Task> insertAct = () => readOnlyRepo.Insert(new VehicleMake());
            Func<Task> deleteAct = () => readOnlyRepo.Delete(1);
            Func<Task> updateAct = () => readOnlyRepo.Update(new VehicleMake());

            // Assert
            await insertAct.Should().ThrowAsync<InvalidOperationException>();
            await deleteAct.Should().ThrowAsync<InvalidOperationException>();
            await updateAct.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
