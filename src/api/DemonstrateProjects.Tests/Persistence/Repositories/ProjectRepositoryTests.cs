using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces;
using DemonstrateProjects.Infrastructure.Persistence;
using DemonstrateProjects.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Persistence.Repositories;

public class ProjectRepositoryTests
{
    private readonly Mock<AppDbContext> _contextStub;
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;

    private readonly ProjectRepository _sut;

    private readonly Project _mainEntity;

    public ProjectRepositoryTests()
    {
        _contextStub = new(new DbContextOptions<AppDbContext>());
        _unitOfWorkStub = new();

        // _contextStub.Setup(x => x.Set<Project>()).Returns(_contextStub.Object.Projects);

        _sut = new(_contextStub.Object);

        _mainEntity = new Project()
        {
            Id = 0,
            Title = "",
            Description = "",
            UserId = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task CreateAsync_ShouldAddToDatabase_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.CreateAsync(_mainEntity)).Verifiable();

        // Act
        await _sut.CreateAsync(_mainEntity);

        // Assert
        _unitOfWorkStub.Verify(x => x.Projects.CreateAsync(_mainEntity), Times.Once);
    }

    [Fact]
    public async Task GetEntitiesAsync_ShouldReturnAllEntities_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.GetEntitiesAsync()).ReturnsAsync(new List<Project>() { _mainEntity }.AsQueryable());

        // Act
        var result = await _sut.GetEntitiesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<Project>>(result);
        Assert.Contains(_mainEntity, result);
    }

    [Fact]

    public async Task GetByUserIdAsync_ShouldReturnEntitiesFromUser_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.GetByUserIdAsync(_mainEntity.UserId)).ReturnsAsync(new List<Project>() { _mainEntity }.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAsync(_mainEntity.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<Project>>(result);
        Assert.Contains(_mainEntity, result);
    }

    [Fact]
    public async Task GetEntityAsync_ShouldReturnEntity_WhenEntityFoundByKey()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.GetEntityAsync(_mainEntity.Id)).ReturnsAsync(_mainEntity);

        // Act
        var result = await _sut.GetEntityAsync(_mainEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_mainEntity, result);
    }

    [Fact]
    public async Task GetEntityAsync_ShouldReturnNull_WhenEntityNotFoundByKey()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.GetEntityAsync(_mainEntity.Id)).ReturnsAsync((Project?)null);

        // Act
        var result = await _sut.GetEntityAsync(_mainEntity.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.GetEntityAsync(_mainEntity.Id)).ReturnsAsync(_mainEntity);
        _unitOfWorkStub.Setup(x => x.Projects.UpdateAsync(_mainEntity.Id, _mainEntity)).Verifiable();

        // Act
        await _sut.UpdateAsync(_mainEntity.Id, _mainEntity);

        // Assert
        _unitOfWorkStub.Verify(x => x.Projects.UpdateAsync(_mainEntity.Id, _mainEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.Projects.GetEntityAsync(_mainEntity.Id)).ReturnsAsync(_mainEntity);
        _unitOfWorkStub.Setup(x => x.Projects.DeleteAsync(_mainEntity.Id)).Verifiable();

        // Act
        await _sut.DeleteAsync(_mainEntity.Id);

        // Assert
        _unitOfWorkStub.Verify(x => x.Projects.DeleteAsync(_mainEntity.Id), Times.Once);
    }
}