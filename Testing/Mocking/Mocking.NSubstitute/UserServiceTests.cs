using FluentAssertions;
using Microsoft.Extensions.Logging;
using Mocking.Shared;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mocking.NSubstitute
{
    public class UserServiceTests
    {
        private readonly IUserService _service;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly MockLogger<UserService> _logging = Substitute.For<MockLogger<UserService>>();

        public UserServiceTests()
        {
            _service = new UserService(_userRepository, _logging);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userName = "Bob Barker";

            _userRepository.GetByIdAsync(userId).Returns(new UserDto { Id = userId.ToString(), UserName = userName });

            // Act
            var user = await _service.GetByIdAsync(userId);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(userId);
            user.UserName.Should().Be(userName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

            // Act
            var user = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldLogAppropriateMessage_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userName = "Bob Barker";

            _userRepository.GetByIdAsync(userId).Returns(new UserDto { Id = userId.ToString(), UserName = userName });

            // Act
            await _service.GetByIdAsync(userId);

            // Assert
            _logging.Received().Log(LogLevel.Information, $"Retrieved a user with Id: {userId}");
            _logging.DidNotReceive().Log(LogLevel.Information, $"Unable to find a user with Id: {userId}");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldLogAppropriateMessage_WhenUserDoesNotExists()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await _service.GetByIdAsync(userId);

            // Assert
            _logging.DidNotReceive().Log(LogLevel.Information, $"Retrieved a user with Id: {userId}");
            _logging.Received().Log(LogLevel.Information, $"Unable to find a user with Id: {userId}");
        }
    }
}