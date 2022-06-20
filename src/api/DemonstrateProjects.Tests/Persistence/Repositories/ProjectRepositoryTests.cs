using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Infrastructure.Persistence;
using DemonstrateProjects.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Persistence.Repositories;

public class ProjectRepositoryTests
{
    private readonly Mock<AppDbContext> _context;
    private readonly Mock<DbSet<Project>> _setStub;

    private readonly ProjectRepository _sut;

    public ProjectRepositoryTests()
    {
        _context = new(new DbContextOptions<AppDbContext>());
        _setStub = new();

        _context.Setup(x => x.Set<Project>()).Returns(_setStub.Object);

        _sut = new(_context.Object);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnAllFromUser_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projs = new List<Project>()
        {
            new() { UserId = userId },
            new() { UserId = Guid.NewGuid() } 
        };

        _setStub.Setup(x => x.AsQueryable()).Returns(projs.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAsync(userId);

        // Assert
        Assert.True(result.All(x => x.UserId == userId));
    }

    [Fact]
    public async Task GetByUserIdAndIndexAsync_ShouldReturnProject_WhenExistentProject()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var index = 1;

        var projs = new List<Project>()
        {
            new() { UserId = userId, Index = index },
            new() { UserId = Guid.NewGuid(), Index = index },
            new() { UserId = userId, Index = 0 } 
        };

        _setStub.Setup(x => x.AsQueryable()).Returns(projs.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAndIndexAsync(userId, index);

        // Assert
        Assert.True(result == projs[0]);
    }

    [Fact]
    public async Task GetByUserIdAndIndexAsync_ShouldReturnNull_WhenInexistentProject()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var index = 1;

        var projs = new List<Project>()
        {
            new() { UserId = Guid.NewGuid(), Index = index },
            new() { UserId = userId, Index = 0 } 
        };

        _setStub.Setup(x => x.AsQueryable()).Returns(projs.AsQueryable());

        // Act
        var result = await _sut.GetByUserIdAndIndexAsync(userId, index);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAllAsync_ShouldDeleteAllFromUser_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projs = new List<Project>()
        {
            new() { UserId = userId },
            new() { UserId = Guid.NewGuid() } 
        };

        _setStub.Setup(x => x.AsQueryable()).Returns(projs.AsQueryable());
        _setStub.Setup(x => x.RemoveRange(projs[0])).Verifiable();

        // Act
        _sut.DeleteAllAsync(userId);

        // Assert
        _setStub.Verify(x => x.RemoveRange(new List<Project>() { projs[0] }), Times.Once);
    }
}