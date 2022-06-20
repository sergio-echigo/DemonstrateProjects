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
    private readonly Mock<DbSet<PersonalReadKey>> _setStub;

    private readonly PersonalReadKeyRepository _sut;

    public PersonalReadKeyRepositoryTests()
    {
        _contextStub = new(new DbContextOptions<AppDbContext>());
        _setStub = new();

        _contextStub.Setup(x => x.Set<PersonalReadKey>()).Returns(_setStub.Object);

        _sut = new(_contextStub.Object);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnAllFromUser_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var keys = new List<PersonalReadKey>()
        {
            new() { UserId = userId },
            new() { UserId = Guid.NewGuid() }
        };

        _setStub.Setup(x => x.AsQueryable()).Returns(keys.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAsync(userId);

        // Assert
        Assert.True(result.All(x => x.UserId == userId));
    }

    [Fact]
    public void DeleteAllAsync_ShouldDeleteAllFromUser_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var keys = new List<PersonalReadKey>()
        {
            new() { UserId = userId },
            new() { UserId = Guid.NewGuid() }
        };

        _setStub.Setup(x => x.AsQueryable()).Returns(keys.AsQueryable());
        _setStub.Setup(x => x.RemoveRange(keys[0])).Verifiable();

        // Act
        _sut.DeleteAllAsync(userId);

        // Assert
        _setStub.Verify(x => x.RemoveRange(new List<PersonalReadKey>() { keys[0] }), Times.Once);
    }
}