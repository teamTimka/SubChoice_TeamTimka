namespace SubChoice.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
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

    public class AdminControllerTests
    {
        private AdminController _testClass;
        private Mock<ISubjectService> _subjectService;
        private Mock<ILoggerService> _loggerService;
        private Mock<IAuthService> _authService;
        private UserManager<User> _userManager;

        public AdminControllerTests()
        {
            _subjectService = new Mock<ISubjectService>();
            _loggerService = new Mock<ILoggerService>();
            _authService = new Mock<IAuthService>();
            _userManager = new UserManager<User>(new Mock<IUserStore<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<IPasswordHasher<User>>().Object, new[] { new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object, new Mock<IUserValidator<User>>().Object }, new[] { new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object, new Mock<IPasswordValidator<User>>().Object }, new Mock<ILookupNormalizer>().Object, new IdentityErrorDescriber(), new Mock<IServiceProvider>().Object, new Mock<ILogger<UserManager<User>>>().Object);
            _testClass = new AdminController(_subjectService.Object, _loggerService.Object, _authService.Object, _userManager);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new AdminController(_subjectService.Object, _loggerService.Object, _authService.Object, _userManager);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public async Task CanCallIndex()
        {
            // Arrange
            _subjectService.Setup(mock => mock.SelectNotApprovedTeachers()).ReturnsAsync(new List<User>());

            // Act
            var result = await _testClass.Index();

            // Assert
            _subjectService.Verify(mock => mock.SelectNotApprovedTeachers());
        }

        [Fact]
        public async Task CanCallAllSubjects()
        {
            // Arrange
            _subjectService.Setup(mock => mock.SelectAllSubjects()).ReturnsAsync(new List<Subject>());

            // Act
            var result = await _testClass.AllSubjects();

            // Assert
            _subjectService.Verify(mock => mock.SelectAllSubjects());
        }

        [Fact]
        public async Task CanCallUsers()
        {
            // Arrange
            _authService.Setup(mock => mock.GetUsers()).ReturnsAsync(new Dictionary<string, List<User>>());

            // Act
            var result = await _testClass.Users();

            // Assert
            _authService.Verify(mock => mock.GetUsers());
        }

        [Fact]
        public async Task CanCallApproveTeacher()
        {
            // Arrange
            var model = new IdDto { Id = new Guid("4ee74171-fa28-43ff-a00d-520d0f552be3") };

            _subjectService.Setup(mock => mock.ApproveUser(It.IsAny<Guid>())).ReturnsAsync(new User { IsSystemAdmin = false, FirstName = "TestValue1868819792", LastName = "TestValue608998334", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("aed107a3-5c2f-44fb-a4f3-12290bd1adb5"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("0d33e288-fa39-4c91-ae11-b5eb3cfbcc58"), CreatedOn = DateTime.UtcNow, IsApproved = true, Teacher = new Teacher { User = default(User), UserId = new Guid("793cac02-b4d6-4dfc-be4e-d5afa50bdc00"), Subjects = new Mock<ICollection<Subject>>().Object }, Student = new Student { User = default(User), UserId = new Guid("e5d2583c-93ef-4017-adeb-a96d6af2cfb6"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } });
            _subjectService.Setup(mock => mock.SelectNotApprovedTeachers()).ReturnsAsync(new List<User>());
            _loggerService.Setup(mock => mock.LogError(It.IsAny<string>())).Verifiable();

            // Act
            var result = await _testClass.ApproveTeacher(model);

            // Assert
            _subjectService.Verify(mock => mock.ApproveUser(It.IsAny<Guid>()));
            _subjectService.Verify(mock => mock.SelectNotApprovedTeachers());
        }

        [Fact]
        public async Task CannotCallApproveTeacherWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.ApproveTeacher(default(IdDto))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CannotCallCreateWithSubjectDataWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.Create(default(SubjectData))).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task CanCallDeleteSubject()
        {
            // Arrange
            var model = new SubjectIdDto { Id = 746068710 };

            _subjectService.Setup(mock => mock.DeleteSubject(It.IsAny<int>())).ReturnsAsync(new Subject { Name = "TestValue1045799685", Description = "TestValue1253801321", StudentsLimit = 1973665342, TeacherId = new Guid("7670c97b-1e47-459f-a367-2f472bc26938"), Teacher = new Teacher { User = new User { IsSystemAdmin = true, FirstName = "TestValue413836403", LastName = "TestValue965201816", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("9f4318b1-e7bc-4978-87e8-b4c9c412be6d"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("b2e635f2-8124-4f3f-bda9-deda3419b0c9"), CreatedOn = DateTime.UtcNow, IsApproved = false, Teacher = default(Teacher), Student = new Student { User = default(User), UserId = new Guid("2314ab22-a5e4-430e-94cc-c2c63ec4f70a"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } }, UserId = new Guid("8fb31f41-945f-4bb5-9d56-0237c1aa18cf"), Subjects = new Mock<ICollection<Subject>>().Object }, StudentSubjects = new Mock<ICollection<StudentSubject>>().Object });
            _subjectService.Setup(mock => mock.SelectAllSubjects()).ReturnsAsync(new List<Subject>());
            _loggerService.Setup(mock => mock.LogError(It.IsAny<string>())).Verifiable();
            _loggerService.Setup(mock => mock.LogInfo(It.IsAny<string>())).Verifiable();

            // Act
            var result = await _testClass.DeleteSubject(model);

            // Assert
            _subjectService.Verify(mock => mock.DeleteSubject(It.IsAny<int>()));
            _subjectService.Verify(mock => mock.SelectAllSubjects());
            _loggerService.Verify(mock => mock.LogInfo(It.IsAny<string>()));
        }

        [Fact]
        public async Task CannotCallDeleteSubjectWithNullModel()
        {
            await FluentActions.Invoking(() => _testClass.DeleteSubject(default(SubjectIdDto))).Should().ThrowAsync<NullReferenceException>();
        }
    }
}