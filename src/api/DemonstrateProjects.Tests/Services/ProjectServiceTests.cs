using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services;
using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Services;

public class ProjectServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly ProjectService _sut;

    private readonly Project _mainEntity;

    public ProjectServiceTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);

        _mainEntity = new()
        {
            Title = "BubbleSpace",
            Description = "QA WebApp.",
            Index = 0,
            UserId = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task AddAsync_ShouldAdd_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var model = new NewProjectModel()
        {
            Title = "SomeTitle",
            Description = "SomeDesc"
        };

        _unitOfWorkStub.Setup(x => x.Projects.Add(It.IsAny<Project>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.AddAsync(userId, model);

        // Assert
        _unitOfWorkStub.Verify(x => x.Projects.Add(It.IsAny<Project>()), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetFromUserAsync_ShouldReturnAllFromUser_WhenExecuted()
    {
        // Arrange
        var expectedUserId = _mainEntity.UserId;
        var expectedIndex = _mainEntity.Index;

        _unitOfWorkStub.Setup(x => x.Projects.GetByUserIdAsync(expectedUserId)).ReturnsAsync(new List<Project>() { _mainEntity }.AsQueryable());

        // Act
        var result = await _sut.GetFromUserAsync(expectedUserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<ProjectModel>>(result);

        Assert.Single(result, x => x.Index == expectedIndex);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnProject_WhenExistent()
    {
        // Arrange
        var userId = _mainEntity.Id;
        var index = _mainEntity.Index;

        _unitOfWorkStub.Setup(x => x.Projects.GetByUserIdAndIndexAsync(userId, index)).ReturnsAsync(_mainEntity);

        // Act
        var result = await _sut.GetAsync(userId, index);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProjectModel>(result);

        /* It will never be null! Well, at least, it is the expected result of the test. */
        Assert.Equal(index, result!.Index);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenInexistent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var index = 2;

        _unitOfWorkStub.Setup(x => x.Projects.GetByUserIdAndIndexAsync(userId, index)).ReturnsAsync((Project?)null);

        // Act
        var result = await _sut.GetAsync(userId, index);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EditAsync_ShouldUpdate_WhenExecuted()
    {
        // Arrange
        var userId = _mainEntity.UserId;
        var index = _mainEntity.Index;

        var model = new EditProjectModel()
        {
            Title = "BubbleSpace",
            Description = "More than a simple QA WebApp."
        };

        _unitOfWorkStub.Setup(x => x.Projects.GetByUserIdAndIndexAsync(userId, index)).ReturnsAsync(_mainEntity);

        _unitOfWorkStub.Setup(x => x.Projects.UpdateAsync(It.IsAny<Project>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.EditAsync(userId, index, model);

        // Assert
        _unitOfWorkStub.Verify(x => x.Projects.UpdateAsync(It.IsAny<Project>()), Times.Once());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_whenExecuted()
    {
        // Arrange
        var userId = _mainEntity.UserId;
        var index = _mainEntity.Index;

        _unitOfWorkStub.Setup(x => x.Projects.GetByUserIdAndIndexAsync(userId, index)).ReturnsAsync(_mainEntity);

        _unitOfWorkStub.Setup(x => x.Projects.DeleteAsync(_mainEntity.Id)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.DeleteAsync(userId, index);

        // Assert
        _unitOfWorkStub.Setup(x => x.Projects.DeleteAsync(_mainEntity.Id)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();
    }
}