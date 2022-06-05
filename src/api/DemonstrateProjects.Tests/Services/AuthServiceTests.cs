using DemonstrateProjects.Application.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace DemonstrateProjects.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _configStub;
    private readonly AuthService _sut;
    
    public AuthServiceTests()
    {
        _configStub = new();

        /* This is not our secret key! */
        _configStub.Setup(x => x.GetSection("JwtConfig:SecretSecureToken").Value).Returns("fewf*&hy443!-=-34R43F");
        _configStub.Setup(x => x.GetSection("JwtConfig:Issuer").Value).Returns("http://localhost:5000");
        _configStub.Setup(x => x.GetSection("JwtConfig:Audience").Value).Returns("http://localhost:3000");

        _sut = new(_configStub.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenExecuted()
    {
        // Arrange
        var username = "arakakiv";
        var isRefresh = false;

        // Act
        var result = _sut.GenerateToken(username, isRefresh);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetUsernameInToken_ShouldReturnUsername_WhenValidToken()
    {
        // Arrange
        var username = "arakakiv";
        var isRefresh = false;

        var token = _sut.GenerateToken(username, isRefresh);

        // Act
        var result = _sut.GetUsernameInToken(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result);
    }

    [Fact]
    public void GetUsernameInToken_ShouldReturnNull_WhenInvalidToken()
    {
        // Arrange
        var token = "4R3R43R43.R43R34RC.3TC34T34";

        // Act
        var result = _sut.GetUsernameInToken(token);

        // Assert
        Assert.Null(result);
    }
}