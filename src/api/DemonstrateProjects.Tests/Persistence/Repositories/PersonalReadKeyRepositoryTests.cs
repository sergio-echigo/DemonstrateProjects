using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces;
using DemonstrateProjects.Infrastructure.Persistence;
using DemonstrateProjects.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Persistence.Repositories;

public class PersonalReadKeyRepositoryTests
{
    private readonly Mock<AppDbContext> _contextStub;
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;

    private readonly PersonalReadKeyRepository _sut;

    private readonly PersonalReadKey _mainEntity;

    public PersonalReadKeyRepositoryTests()
    {
        _contextStub = new(new DbContextOptions<AppDbContext>());
        _unitOfWorkStub = new();

        // _contextStub.Setup(x => x.Set<Project>()).Returns(_contextStub.Object.Projects);

        _sut = new(_contextStub.Object);

        _mainEntity = new()
        {
            UserId = Guid.NewGuid(),
            ExpiresWhen = DateTimeOffset.MaxValue
        };
    }

    [Fact]
    public async Task CreateAsync_ShouldAddToDatabase_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.CreateAsync(_mainEntity)).Verifiable();

        // Act
        await _sut.CreateAsync(_mainEntity);

        // Assert
        _unitOfWorkStub.Verify(x => x.PersonalReadKeys.CreateAsync(_mainEntity), Times.Once);
    }

    [Fact]
    public async Task GetEntitiesAsync_ShouldReturnAllEntities_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntitiesAsync()).ReturnsAsync(new List<PersonalReadKey>() { _mainEntity }.AsQueryable());

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
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetByUserIdAsync(_mainEntity.UserId)).ReturnsAsync(new List<PersonalReadKey>() { _mainEntity }.AsQueryable());

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
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(_mainEntity.Key)).ReturnsAsync(_mainEntity);

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
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(_mainEntity.Key)).ReturnsAsync((PersonalReadKey?)null);

        // Act
        var result = await _sut.GetEntityAsync(_mainEntity.Key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(_mainEntity.Key)).ReturnsAsync(_mainEntity);
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.UpdateAsync(_mainEntity.Key, _mainEntity)).Verifiable();

        // Act
        await _sut.UpdateAsync(_mainEntity.Key, _mainEntity);

        // Assert
        _unitOfWorkStub.Verify(x => x.PersonalReadKeys.UpdateAsync(_mainEntity.Key, _mainEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(_mainEntity.Key)).ReturnsAsync(_mainEntity);
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.DeleteAsync(_mainEntity.Key)).Verifiable();

        // Act
        await _sut.DeleteAsync(_mainEntity.Key);

        // Assert
        _unitOfWorkStub.Verify(x => x.PersonalReadKeys.DeleteAsync(_mainEntity.Key), Times.Once);
    }
}