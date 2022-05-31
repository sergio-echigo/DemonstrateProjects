using DemonstrateProjects.Core.Interfaces.Repositories;
using DemonstrateProjects.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Persistence;

public class UnitOfWorkTests
{
    private readonly Mock<AppDbContext> _contextStub;
    private readonly UnitOfWork _sut;

    public UnitOfWorkTests()
    {
        _contextStub = new(new DbContextOptions<AppDbContext>());
        _sut = new(_contextStub.Object);
    }

    [Fact]
    public void GetProjects_ShouldReturnNotNull_WhenExecuted()
    {
        // Arrange
        
        // Act
        var result = _sut.Projects;

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IProjectRepository>(result);
    }

    [Fact]
    public void GetPersonalReadKeys_ShouldReturnNotNull_WhenExecuted()
    {
        // Arrange
        
        // Act
        var result = _sut.PersonalReadKeys;

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IPersonalReadKeyRepository>(result);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChangesOnContext_WhenExecuted()
    {
        // Arrange
        _contextStub.Setup(x => x.SaveChangesAsync(default)).Verifiable();

        // Act
        await _sut.SaveChangesAsync();

        // Assert
        _contextStub.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}