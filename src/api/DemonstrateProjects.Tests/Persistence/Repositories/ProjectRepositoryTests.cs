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
    private readonly Mock<DbSet<Project>> _dbSetStub;

    private readonly ProjectRepository _sut;

    private readonly Project _mainEntity;

    public ProjectRepositoryTests()
    {
        _contextStub = new(new DbContextOptions<AppDbContext>());
        _dbSetStub = new();

        _contextStub.Setup(x => x.Set<Project>()).Returns(_dbSetStub.Object);

        _sut = new(_contextStub.Object);

        _mainEntity = new Project()
        {
            Title = "",
            Description = "",
            Index = 0,
            UserId = Guid.NewGuid(),
            Img = new byte[100]
        };
    }

    [Fact]
    public async Task Add_ShouldAddToDatabase_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.Add(_mainEntity)).Verifiable();

        // Act
        await _sut.Add(_mainEntity);

        // Assert
        _dbSetStub.Verify(x => x.Add(_mainEntity), Times.Once);
    }

    [Fact]
    public async Task GetEntitiesAsync_ShouldReturnAllEntities_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.AsQueryable()).Returns(new List<Project>() { _mainEntity }.AsQueryable());

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
        _dbSetStub.Setup(x => x.AsQueryable()).Returns(new List<Project>() { _mainEntity }.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAsync(_mainEntity.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<Project>>(result);
        Assert.Contains(_mainEntity, result);
    }

    [Fact]
    public async Task GetByUserIdAndIndex_ShouldReturnEntity_WhenEntityFound()
    {
        // Arrange
        _dbSetStub.Setup(x => x.AsQueryable()).Returns(new List<Project>() { _mainEntity }.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAndIndexAsync(_mainEntity.UserId, _mainEntity.Index);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Project>(result);
        Assert.Equal(_mainEntity, result);
    }

    [Fact]
    public async Task GetByUserIdAndIndex_ShouldReturnNull_WhenEntityNotFound()
    {
        // Arrange
        _dbSetStub.Setup(x => x.AsQueryable()).Returns(new List<Project>().AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAndIndexAsync(_mainEntity.UserId, _mainEntity.Index);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetEntityAsync_ShouldReturnEntity_WhenEntityFoundByKey()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Id)).ReturnsAsync(_mainEntity);

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
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Id)).ReturnsAsync((Project?)null);

        // Act
        var result = await _sut.GetEntityAsync(_mainEntity.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Id)).ReturnsAsync(_mainEntity);
        _dbSetStub.Setup(x => x.Update(_mainEntity)).Verifiable();

        // Act
        await _sut.UpdateAsync(_mainEntity);

        // Assert
        _dbSetStub.Verify(x => x.Update(_mainEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Id)).ReturnsAsync(_mainEntity);
        _dbSetStub.Setup(x => x.Remove(_mainEntity)).Verifiable();

        // Act
        await _sut.DeleteAsync(_mainEntity.Id);

        // Assert
        _dbSetStub.Verify(x => x.Remove(_mainEntity), Times.Once);
    }
}