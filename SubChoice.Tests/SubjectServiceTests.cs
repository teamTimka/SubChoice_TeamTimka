namespace SubChoice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Moq;
    using SubChoice.Core.Data.Dto;
    using SubChoice.Core.Data.Entities;
    using SubChoice.Core.Interfaces.DataAccess;
    using SubChoice.Services;
    using Xunit;

    public class SubjectServiceTests
    {
        private SubjectService _testClass;
        private Mock<IMapper> _mapper;
        private Mock<IRepoWrapper> _repository;

        public SubjectServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<IRepoWrapper>();
            _testClass = new SubjectService(_mapper.Object, _repository.Object);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new SubjectService(_mapper.Object, _repository.Object);

            // Assert
            Assert.NotNull(instance);
        }

        [Fact]
        public async Task CanCallSelectAllSubjects()
        {
            // Arrange
            _repository.Setup(mock => mock.Subjects).Returns(new Mock<ISubjectRepository>().Object);

            // Act
            var result = await _testClass.SelectAllSubjects();
        }

        [Fact]
        public async Task CanCallCreateSubject()
        {
            // Arrange
            var data = new SubjectData { Name = "TestValue1245322857", StudentsLimit = 234860181, Description = "TestValue1526146585", Id = 1088093637, TeacherId = new Guid("31b16de9-35b3-438d-b625-c0c86667aab1") };

            _repository.Setup(mock => mock.SaveChanges()).Returns(88307619);
            _repository.Setup(mock => mock.Subjects).Returns(new Mock<ISubjectRepository>().Object);

            // Act
            var result = await _testClass.CreateSubject(data);

            // Assert
            _repository.Verify(mock => mock.SaveChanges());
        }

        [Fact]
        public async Task CannotCallCreateSubjectWithNullData()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => _testClass.CreateSubject(default(SubjectData)));
        }

        [Fact]
        public async Task CreateSubjectPerformsMapping()
        {
            // Arrange
            var data = new SubjectData { Name = "TestValue1469486670", StudentsLimit = 570838934, Description = "TestValue2028142514", Id = 96870841, TeacherId = new Guid("7c4930e6-173c-4ea4-9d77-4321cc69c7cf") };
            var subject = new Subject { Name = "TestValue1469486670", Description = "TestValue2028142514", StudentsLimit = 570838934, TeacherId = new Guid("7c4930e6-173c-4ea4-9d77-4321cc69c7cf"), Teacher = new Teacher { User = new User { IsSystemAdmin = true, FirstName = "TestValue413836403", LastName = "TestValue965201816", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("9f4318b1-e7bc-4978-87e8-b4c9c412be6d"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("b2e635f2-8124-4f3f-bda9-deda3419b0c9"), CreatedOn = DateTime.UtcNow, IsApproved = false, Teacher = default(Teacher), Student = new Student { User = default(User), UserId = new Guid("2314ab22-a5e4-430e-94cc-c2c63ec4f70a"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } }, UserId = new Guid("8fb31f41-945f-4bb5-9d56-0237c1aa18cf"), Subjects = new Mock<ICollection<Subject>>().Object }, StudentSubjects = new Mock<ICollection<StudentSubject>>().Object };
            _mapper.Setup(mock => mock.Map<SubjectData, Subject>(data)).Returns(subject);
            _repository.Setup(m => m.Subjects.Create(subject)).Returns(subject);

            // Act
            var result = await _testClass.CreateSubject(data);

            // Assert
            Assert.Same(data.Name, result.Name);
            Assert.Equal(data.StudentsLimit, result.StudentsLimit);
            Assert.Same(data.Description, result.Description);
            Assert.Equal(data.TeacherId, result.TeacherId);
        }

        [Fact]
        public async Task CannotCallUpdateSubjectWithNullData()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => _testClass.UpdateSubject(2011121798, default(SubjectData)));
        }

        [Fact]
        public async Task CanCallDeleteSubject()
        {
            // Arrange
            var id = 65115581;

            _repository.Setup(mock => mock.SaveChanges()).Returns(827737863);
            _repository.Setup(mock => mock.Subjects).Returns(new Mock<ISubjectRepository>().Object);

            // Act
            var result = await _testClass.DeleteSubject(id);

            // Assert
            _repository.Verify(mock => mock.SaveChanges());
        }

        [Fact]
        public async Task CanCallSelectAllByTeacherId()
        {
            // Arrange
            var teacherId = new Guid("7125858d-e63b-4387-8967-b1b180acdc84");

            _repository.Setup(mock => mock.Subjects).Returns(new Mock<ISubjectRepository>().Object);

            // Act
            var result = await _testClass.SelectAllByTeacherId(teacherId);
        }

        [Fact]
        public async Task CanCallSelectAllByStudentId()
        {
            // Arrange
            var studentId = new Guid("a7bb256a-bc5c-4072-8bbd-3e216354eec4");

            _repository.Setup(mock => mock.Subjects).Returns(new Mock<ISubjectRepository>().Object);

            // Act
            var result = await _testClass.SelectAllByStudentId(studentId);
        }

        [Fact]
        public async Task CanCallSelectAllStudentsSubjects()
        {
            // Arrange
            var subjectId = 1609807445;

            _repository.Setup(mock => mock.Students).Returns(new Mock<IStudentRepository>().Object);

            // Act
            var result = await _testClass.SelectAllStudentsSubjects(subjectId);
        }

        [Fact]
        public async Task CanCallRegisterStudent()
        {
            // Arrange
            var studentId = new Guid("a67a660e-6afb-4317-a014-a05f7c0556c5");
            var subjectId = 103048697;

            _repository.Setup(mock => mock.SaveChanges()).Returns(1001085904);
            _repository.Setup(mock => mock.StudentSubjects).Returns(new Mock<IStudentSubjectRepository>().Object);

            // Act
            var result = await _testClass.RegisterStudent(studentId, subjectId);

            // Assert
            _repository.Verify(mock => mock.SaveChanges());
        }

        [Fact]
        public async Task RegisterStudentPerformsMapping()
        {
            // Arrange
            var studentId = new Guid("b63feb27-55f6-4289-8d0b-587d85269869");
            var subjectId = 165212457;
            _repository.Setup(m => m.StudentSubjects.Create(It.IsAny<StudentSubject>()));

            // Act
            var result = await _testClass.RegisterStudent(studentId, subjectId);

            // Assert
            Assert.Equal(studentId, result.StudentId);
            Assert.Equal(subjectId, result.SubjectId);
        }

        [Fact]
        public async Task CanCallUnRegisterStudent()
        {
            // Arrange
            var studentId = new Guid("c5406611-d66b-4714-b59d-b3f728096ab4");
            var subjectId = 295019770;

            _repository.Setup(mock => mock.SaveChanges()).Returns(639242838);
            _repository.Setup(mock => mock.StudentSubjects).Returns(new Mock<IStudentSubjectRepository>().Object);

            // Act
            var result = await _testClass.UnRegisterStudent(studentId, subjectId);

            // Assert
            _repository.Verify(mock => mock.SaveChanges());
        }

        [Fact]
        public async Task UnRegisterStudentPerformsMapping()
        {
            // Arrange
            var studentId = new Guid("c474c5ad-d012-4451-a632-bb60e65e7ddd");
            var subjectId = 1222989630;
            _repository.Setup(m => m.StudentSubjects.Delete(It.IsAny<Guid>(), It.IsAny<int>()));

            // Act
            var result = await _testClass.UnRegisterStudent(studentId, subjectId);

            // Assert
            _repository.Verify(m => m.SaveChanges());
        }

        [Fact]
        public async Task CanCallSelectNotApprovedTeachers()
        {
            // Arrange
            _repository.Setup(mock => mock.Users).Returns(new Mock<IUserRepository>().Object);

            // Act
            var result = await _testClass.SelectNotApprovedTeachers();
        }

        [Fact]
        public async Task CanCallApproveUser()
        {
            // Arrange
            var id = new Guid("a7d55a77-df55-4047-8563-87e78a59f8cc");
            var user = new User { IsSystemAdmin = true, FirstName = "TestValue1338163780", LastName = "TestValue1847869697", InactiveAt = DateTime.UtcNow, ModifiedBy = new Guid("e7b8e7a7-89b7-44a3-9cbd-74bd087a8bcf"), ModifiedOn = DateTime.UtcNow, CreatedBy = new Guid("a6dee632-e14c-4f3b-b3f7-6c0de6bcdd39"), CreatedOn = DateTime.UtcNow, IsApproved = false, Teacher = new Teacher { User = default(User), UserId = new Guid("7546ef86-b484-40f4-a4ff-2f645a5b78c6"), Subjects = new Mock<ICollection<Subject>>().Object }, Student = new Student { User = default(User), UserId = new Guid("6f608fd3-de10-4386-857e-4ccc72ceca32"), StudentSubjects = new Mock<ICollection<StudentSubject>>().Object } };

            _repository.Setup(mock => mock.SaveChanges()).Returns(67993088);
            _repository.Setup(mock => mock.Users).Returns(new Mock<IUserRepository>().Object);
            _repository.Setup(mock => mock.Users.SelectById(id, true)).Returns(user);

            // Act
            var result = await _testClass.ApproveUser(id);

            // Assert
            _repository.Verify(mock => mock.SaveChanges());
        }

        [Fact]
        public async Task CanCallCheckIfRecordStudentSubjectExists()
        {
            // Arrange
            var subId = 977275226;
            var studId = new Guid("36b5db66-f07e-490c-b255-04b17594df0d");

            _repository.Setup(mock => mock.StudentSubjects).Returns(new Mock<IStudentSubjectRepository>().Object);

            // Act
            var result = await _testClass.CheckIfRecordStudentSubjectExists(subId, studId);

            // Assert
            Assert.False(result);
        }
    }
}