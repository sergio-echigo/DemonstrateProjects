using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services;
using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Services;

public class PersonalReadKeyServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly PersonalReadKeyService _sut;

    private readonly PersonalReadKey _mainEntity;

    public PersonalReadKeyServiceTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);

        _mainEntity = new()
        {
            UserId = Guid.NewGuid(),
            ExpiresWhen = DateTimeOffset.Now.AddDays(30)
        };
    }

    [Fact]
    public async Task CreateAsync__ShouldReturnNewKey_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var model = new NewPersonalReadKeyModel() { ExpiresWhen = DateTime.Now.AddDays(15) };

        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.Add(It.IsAny<PersonalReadKey>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.CreateAsync(userId, model);

        //Assert
        _unitOfWorkStub.Verify(x => x.PersonalReadKeys.Add(It.IsAny<PersonalReadKey>()), Times.Once());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once());
    }

    [Fact]
    public async Task GetFromUser_ShouldReturnAllFromUser_WhenExecuted()
    {
        // Arrange
        var userId = _mainEntity.UserId;
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<PersonalReadKey>() { _mainEntity }
                .AsQueryable());

        // Act
        var result = await _sut.GetFromUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<PersonalReadKeyModel>>(result);

        Assert.Single(result, x => x.Key == _mainEntity.Key);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnKey_WhenExistent()
    {
        // Arrange
        var key = _mainEntity.Key;
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(key)).ReturnsAsync(_mainEntity);

        // Act
        var result = await _sut.GetAsync(key);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PersonalReadKeyModel>(result);

        /* We're verifying the existence before! */
        Assert.Equal(_mainEntity.Key, result!.Key);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenInexistent()
    {
        // Arrange
        var key = _mainEntity.Key;
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(key)).ReturnsAsync((PersonalReadKey?)null);

        // Act
        var result = await _sut.GetAsync(key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteKey_WhenExecuted()
    {
        // Arrange
        var key = _mainEntity.Key;
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.GetEntityAsync(key)).ReturnsAsync(_mainEntity);

        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.DeleteAsync(key)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.DeleteAsync(key);

        // Assert
        _unitOfWorkStub.Verify(x => x.PersonalReadKeys.DeleteAsync(key), Times.Once());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAllFromUserAsync_ShouldDeleteAll_WhenExecuted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        _unitOfWorkStub.Setup(x => x.PersonalReadKeys.DeleteAllFromUserAsync(userId)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.DeleteAllFromUserAsync(userId);

        // Assert
        _unitOfWorkStub.Verify(x => x.PersonalReadKeys.DeleteAllFromUserAsync(userId), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}