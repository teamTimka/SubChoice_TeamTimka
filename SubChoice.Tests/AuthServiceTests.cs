namespace SubChoice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SubChoice.Core.Data.Dto;
    using SubChoice.Core.Data.Entities;
    using SubChoice.Core.Interfaces.DataAccess;
    using SubChoice.Services;
    using Xunit;

    public class AuthServiceTests
    {
        private AuthService _testClass;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private Mock<IMapper> _mapper;
        private Mock<IRepoWrapper> _repository;

        public AuthServiceTests()
        {
            _userManager = new UserManager<User>(new Mock<IUserStore<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<User>>().Object, new[] { new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object }, new[] { new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object }, new Mock<ILookupNormalizer>().Object, new IdentityErrorDescriber(), new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<User>>>().Object);
            _signInManager = new SignInManager<User>(new UserManager<User>(new Mock<IUserStore<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<User>>().Object, new[] { new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object }, new[] { new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object }, new Mock<ILookupNormalizer>().Object, new IdentityErrorDescriber(), new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<User>>>().Object), new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<User>>>().Object, new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<User>>().Object);
            _mapper = new Mock<IMapper>();
            _repository = new Mock<IRepoWrapper>();
            _testClass = new AuthService(_userManager, _signInManager, _mapper.Object, _repository.Object);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new AuthService(_userManager, _signInManager, _mapper.Object, _repository.Object);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public async Task CannotCallSignInAsyncWithNullLoginDto()
        {
            await FluentActions.Invoking(() => _testClass.SignInAsync(default(LoginDto))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CannotCallCreateUserAsyncWithNullRegisterDto()
        {
            await FluentActions.Invoking(() => _testClass.CreateUserAsync(default(RegisterDto))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CannotCallAddRoleAsyncWithNullRegisterDto()
        {
            await FluentActions.Invoking(() => _testClass.AddRoleAsync(default(RegisterDto))).Should().ThrowAsync<NullReferenceException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CannotCallGetUserByEmailWithInvalidEmail(string value)
        {
            await FluentActions.Invoking(() => _testClass.GetUserByEmail(value)).Should().ThrowAsync<NotSupportedException>();
        }

        [Fact]
        public void CanCallCreateTeacher()
        {
            // Arrange
            var user = new User { IsSystemAdmin = true, FirstName = "TestValue1338163780", LastName = "TestValue1847869697", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("e7b8e7a7-89b7-44a3-9cbd-74bd087a8bcf"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("a6dee632-e14c-4f3b-b3f7-6c0de6bcdd39"), CreatedOn = DateTime.UtcNow, IsApproved = false, Teacher = new Teacher { User = default(User), UserId = new Guid("7546ef86-b484-40f4-a4ff-2f645a5b78c6"), Subjects = new Mock<ICollection<Subject>>().Object }, Student = new Student { User = default(User), UserId = new Guid("6f608fd3-de10-4386-857e-4ccc72ceca32"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } };

            _repository.Setup(mock => mock.Teachers).Returns(new Mock<ITeacherRepository>().Object);

            var result = _testClass.CreateTeacher(user);
        }

        [Fact]
        public void CannotCallCreateTeacherWithNullUser()
        {
            FluentActions.Invoking(() => _testClass.CreateTeacher(default(User))).Should().Throw<NullReferenceException>();
        }
    }
}