using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Infrastructure.Persistence;
using DemonstrateProjects.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Persistence.Repositories;

public class PersonalReadKeyRepositoryTests
{
    private readonly Mock<AppDbContext> _contextStub;
    private readonly Mock<DbSet<PersonalReadKey>> _dbSetStub;

    private readonly PersonalReadKeyRepository _sut;

    private readonly PersonalReadKey _mainEntity;

    public PersonalReadKeyRepositoryTests()
    {
        _contextStub = new(new DbContextOptions<AppDbContext>());
        _dbSetStub = new();

        _contextStub.Setup(x => x.Set<PersonalReadKey>()).Returns(_dbSetStub.Object);

        _sut = new(_contextStub.Object);

        _mainEntity = new()
        {
            UserId = Guid.NewGuid(),
            ExpiresWhen = DateTimeOffset.MaxValue
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
        var listOf = new List<PersonalReadKey>() { _mainEntity }.AsQueryable();
        _dbSetStub.Setup(x => x.AsQueryable()).Returns(listOf);

        // Act
        var result = await _sut.GetEntitiesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<PersonalReadKey>>(result);
        Assert.Contains(_mainEntity, result);
    }

    [Fact]

    public async Task GetByUserIdAsync_ShouldReturnEntitiesFromUser_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.AsQueryable()).Returns(new List<PersonalReadKey>() { _mainEntity }.AsQueryable<PersonalReadKey>());

        // Act
        var result = await _sut.GetByUserIdAsync(_mainEntity.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<PersonalReadKey>>(result);
        Assert.Contains(_mainEntity, result);
    }

    [Fact]
    public async Task GetEntityAsync_ShouldReturnEntity_WhenEntityFoundByKey()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Key)).ReturnsAsync(_mainEntity);

        // Act
        var result = await _sut.GetEntityAsync(_mainEntity.Key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_mainEntity, result);
    }

    [Fact]
    public async Task GetEntityAsync_ShouldReturnNull_WhenEntityNotFoundByKey()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Key)).ReturnsAsync((PersonalReadKey?)null);

        // Act
        var result = await _sut.GetEntityAsync(_mainEntity.Key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Key)).ReturnsAsync(_mainEntity);
        _dbSetStub.Setup(x => x.Update(_mainEntity)).Verifiable();

        // Act
        await _sut.UpdateAsync(_mainEntity);

        // Assert
        _dbSetStub.Verify(x => x.Update(_mainEntity), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenExecuted()
    {
        // Arrange
        _dbSetStub.Setup(x => x.FindAsync(_mainEntity.Key)).ReturnsAsync(_mainEntity);
        _dbSetStub.Setup(x => x.Remove(_mainEntity)).Verifiable();

        // Act
        await _sut.DeleteAsync(_mainEntity.Key);

        // Assert
        _dbSetStub.Verify(x => x.Remove(_mainEntity), Times.Once);
    }
}