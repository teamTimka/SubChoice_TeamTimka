namespace SubChoice.Tests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using SubChoice.Controllers;
    using SubChoice.Core.Data.Dto;
    using SubChoice.Core.Interfaces.Services;
    using Xunit;

    public class AuthControllerTests
    {
        private AuthController _testClass;
        private Mock<IAuthService> _authService;
        private Mock<ILoggerService> _loggerService;

        public AuthControllerTests()
        {
            _authService = new Mock<IAuthService>();
            _loggerService = new Mock<ILoggerService>();
            _testClass = new AuthController(_authService.Object, _loggerService.Object);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new AuthController(_authService.Object, _loggerService.Object);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public async Task CanCallRegisterWithRegisterDto()
        {
            // Arrange
            var model = new RegisterDto { FirstName = "TestValue1785397405", LastName = "TestValue940580864", Role = "TestValue527974710", Email = "TestValue1275749692", Password = "TestValue808843845", ConfirmPassword = "TestValue1274449202" };

            _authService.Setup(mock => mock.CreateUserAsync(It.IsAny<RegisterDto>())).ReturnsAsync(new IdentityResult());

            // Act
            var result = await _testClass.Register(model);

            // Assert
            _authService.Verify(mock => mock.CreateUserAsync(It.IsAny<RegisterDto>()));
        }

        [Fact]
        public async Task CannotCallRegisterWithRegisterDtoWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.Register(default(RegisterDto))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CanCallLoginWithLoginDto()
        {
            // Arrange
            var model = new LoginDto { Email = "TestValue2030603361", Password = "TestValue820838619", RememberMe = true };

            _authService.Setup(mock => mock.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(new SignInResult());
            _loggerService.Setup(mock => mock.LogInfo(It.IsAny<string>())).Verifiable();
            _loggerService.Setup(mock => mock.LogError(It.IsAny<string>())).Verifiable();

            // Act
            var result = await _testClass.Login(model);

            // Assert
            _authService.Verify(mock => mock.SignInAsync(It.IsAny<LoginDto>()));
        }
    }
}