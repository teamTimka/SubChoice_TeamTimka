namespace SubChoice.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using SubChoice.Controllers;
    using SubChoice.Core.Data.Dto;
    using SubChoice.Core.Data.Entities;
    using SubChoice.Core.Interfaces.Services;
    using Xunit;

    public class HomeControllerTests
    {
        private List<User> _users = new List<User>
        {
            new User { IsSystemAdmin = true, FirstName = "TestValue1338163780", LastName = "TestValue1847869697", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("e7b8e7a7-89b7-44a3-9cbd-74bd087a8bcf"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("a6dee632-e14c-4f3b-b3f7-6c0de6bcdd39"), CreatedOn = DateTime.UtcNow, IsApproved = false, Teacher = new Teacher { User = default(User), UserId = new Guid("7546ef86-b484-40f4-a4ff-2f645a5b78c6"), Subjects = new Mock<ICollection<Subject>>().Object }, Student = new Student { User = default(User), UserId = new Guid("6f608fd3-de10-4386-857e-4ccc72ceca32"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } },
            new User { IsSystemAdmin = true, FirstName = "TestValue1338163781", LastName = "TestValue1847869698", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("e7b8e7a7-89b7-44a3-9cbd-74bd087a8bcf"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("a6dee632-e14c-4f3b-b3f7-6c0de6bcdd39"), CreatedOn = DateTime.UtcNow, IsApproved = false, Teacher = new Teacher { User = default(User), UserId = new Guid("7546ef86-b484-40f4-a4ff-2f645a5b78c7"), Subjects = new Mock<ICollection<Subject>>().Object }, Student = new Student { User = default(User), UserId = new Guid("6f608fd3-de10-4386-857e-4ccc72ceca32"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } }
        };

        private HomeController _testClass;
        private Mock<ILoggerService> _loggerService;
        private Mock<ISubjectService> _subjectService;
        private Mock<IAuthService> _authService;
        private UserManager<User> _userManager;

        public HomeControllerTests()
        {
            _loggerService = new Mock<ILoggerService>();
            _subjectService = new Mock<ISubjectService>();
            _authService = new Mock<IAuthService>();
            _userManager = new UserManager<User>(new Mock<IUserStore<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<User>>().Object, new[] { new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object }, new[] { new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object }, new Mock<ILookupNormalizer>().Object, new IdentityErrorDescriber(), new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<User>>>().Object);
            _testClass = new HomeController(_loggerService.Object, _subjectService.Object, _authService.Object, _userManager);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new HomeController(_loggerService.Object, _subjectService.Object, _authService.Object, _userManager);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public async Task CannotCallCreateWithSubjectDataWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.Create(default(SubjectData))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CannotCallEditSubjectWithSubjectDataWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.EditSubject(default(SubjectData))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CannotCallDeleteSubjectWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.DeleteSubject(default(SubjectIdDto))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CannotCallRegisterWithNullSubId()
        {
            await FluentActions.Invoking(() => _testClass.Register(default(SubjectIdDto))).Should().ThrowAsync<ArgumentNullException>().WithParameterName("principal");
        }

        [Fact]
        public async Task CannotCallUnregisterWithNullSubId()
        {
            await FluentActions.Invoking(() => _testClass.Unregister(default(SubjectIdDto))).Should().ThrowAsync<ArgumentNullException>().WithParameterName("principal");
        }

        [Fact]
        public async Task CanCallStudentsSubject()
        {
            // Arrange
            var id = 1054530659;

            _subjectService.Setup(mock => mock.SelectAllStudentsSubjects(It.IsAny<int>())).ReturnsAsync(new List<Student>());

            // Act
            var result = await _testClass.StudentsSubject(id);

            // Assert
            _subjectService.Verify(mock => mock.SelectAllStudentsSubjects(It.IsAny<int>()));
        }

        [Fact]
        public async Task CanCallSubjectDetail()
        {
            // Arrange
            var id = 1719736369;

            _subjectService.Setup(mock => mock.SelectById(It.IsAny<int>())).ReturnsAsync(new Subject { Name = "TestValue1278056258", Description = "TestValue1885971239", StudentsLimit = 51173250, TeacherId = new Guid("cbab5f91-78cd-4a11-8dbd-ad05800db113"), Teacher = new Teacher { User = new User { IsSystemAdmin = true, FirstName = "TestValue1931324095", LastName = "TestValue1021170261", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("b3989e9e-07d0-4bb4-8f32-72603b8d7ac7"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("99b70fbf-563f-424c-b49a-a71d72b0b32b"), CreatedOn = DateTime.UtcNow, IsApproved = true, Teacher = default(Teacher), Student = new Student { User = default(User), UserId = new Guid("dc19a121-4fa7-452c-a9ee-64634d95ae16"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } }, UserId = new Guid("00c28173-4172-4d90-a456-6b45a3267bc6"), Subjects = new Mock<ICollection<Subject>>().Object }, StudentSubjects = new Mock<ICollection<StudentSubject>>().Object });

            // Act
            var result = await _testClass.SubjectDetail(id);

            // Assert
            _subjectService.Verify(mock => mock.SelectById(It.IsAny<int>()));
        }
    }
}