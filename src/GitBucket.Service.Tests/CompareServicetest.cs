using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GitBucket.Core;
using Moq;
using Octokit;
using Octokit.Internal;
using Xunit;

namespace GitBucket.Service.Tests
{
    public class CompareServicetest
    {
        private readonly Mock<IGitHubClient> _client;
        private readonly CompareService _service;

        public CompareServicetest()
        {
            _client = new Mock<IGitHubClient>();
            _client
                .SetupGet(c => c.Connection.BaseAddress)
                .Returns(new Uri("http://localhost:8080/api/v3/"));
            _client
                .Setup(c => c.Repository.GetAllForOrg(It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<FakeRepositoory>(new[] { new FakeRepositoory("root/test1", "test1", "root") }));
            _client
                .Setup(c => c.Repository.Branch.GetAll(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<FakeBranch>(new[] { new FakeBranch("master"), new FakeBranch("develop") }));

            _service = new CompareService(FakeConsole);
        }

        public FakeConsole FakeConsole { get; } = new FakeConsole();

        [Fact]
        public async void Develop_Has_Unmerged_Commits()
        {
            // Given
            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u => u.AbsoluteUri == "http://localhost:8080/root/test1/compare/root:master...root:develop"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "test"));

            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u => u.AbsoluteUri == "http://localhost:8080/root/test1/compare/root:develop...root:master"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "There isn't anything to compare."));

            var options = new CompareOptions { Owners = new[] { "root" } };

            // When
            var result = await _service.Execute(options, _client.Object);
            Assert.Equal(0, result);
            Assert.Single(FakeConsole.Messages);
            Assert.Equal("develop in root/test1 has commits which has not merged into master.", FakeConsole.Messages.First());
        }

        [Fact]
        public async void Master_Has_Unmerged_Commits()
        {
            // Given
            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u => u.AbsoluteUri == "http://localhost:8080/root/test1/compare/root:develop...root:master"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "test"));

            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u => u.AbsoluteUri == "http://localhost:8080/root/test1/compare/root:master...root:develop"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "There isn't anything to compare."));

            var options = new CompareOptions { Owners = new[] { "root" } };

            // When
            var result = await _service.Execute(options, _client.Object);
            Assert.Equal(0, result);
            Assert.Single(FakeConsole.Messages);
            Assert.Equal("master in root/test1 has commits which has not merged into develop.", FakeConsole.Messages.First());
        }

        [Fact]
        public async void Develop_And_Master_Have_Unmerged_Commits()
        {
            // Given
            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u => u.AbsoluteUri == "http://localhost:8080/root/test1/compare/root:develop...root:master"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "test"));

            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u => u.AbsoluteUri == "http://localhost:8080/root/test1/compare/root:master...root:develop"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "test"));

            var options = new CompareOptions { Owners = new[] { "root" } };

            // When
            var result = await _service.Execute(options, _client.Object);
            Assert.Equal(0, result);
            Assert.Equal(2, FakeConsole.Messages.Count);
            Assert.Equal("develop in root/test1 has commits which has not merged into master.", FakeConsole.Messages[0]);
            Assert.Equal("master in root/test1 has commits which has not merged into develop.", FakeConsole.Messages[1]);
        }

        [Fact]
        public async void Multiple_Owners_And_Repositories_Have_Unmerged_Commits()
        {
            // Given
            _client
                .Setup(c => c.Connection.GetHtml(It.IsAny<Uri>(), It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "There isn't anything to compare."));

            _client
                .Setup(c => c.Connection.GetHtml(
                    It.Is<Uri>(u =>
                        u.AbsoluteUri == "http://localhost:8080/user1/test1/compare/user1:master...user1:develop"
                        || u.AbsoluteUri == "http://localhost:8080/user1/test2/compare/user1:master...user1:develop"
                        || u.AbsoluteUri == "http://localhost:8080/user2/test1/compare/user2:master...user2:develop"
                        || u.AbsoluteUri == "http://localhost:8080/user2/test2/compare/user2:master...user2:develop"),
                    It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(new ApiResponse<string>(new Mock<IResponse>().Object, "test"));

            _client
                .Setup(c => c.Repository.GetAllForOrg(It.IsAny<string>()))
                .ReturnsAsync((string org) => new ReadOnlyCollection<FakeRepositoory>(new[]
                {
                    new FakeRepositoory($"{org}/test1", "test1", org),
                    new FakeRepositoory($"{org}/test2", "test2", org),
                }));

            var options = new CompareOptions { Owners = new[] { "user1", "user2" } };

            // When
            var result = await _service.Execute(options, _client.Object);
            Assert.Equal(0, result);
            Assert.Equal(4, FakeConsole.Messages.Count);
            Assert.Equal("develop in user1/test1 has commits which has not merged into master.", FakeConsole.Messages[0]);
            Assert.Equal("develop in user1/test2 has commits which has not merged into master.", FakeConsole.Messages[1]);
            Assert.Equal("develop in user2/test1 has commits which has not merged into master.", FakeConsole.Messages[2]);
            Assert.Equal("develop in user2/test2 has commits which has not merged into master.", FakeConsole.Messages[3]);
        }

        [Fact]
        public async void Should_Skip_Repositories()
        {
            // Given
            _client
                .Setup(c => c.Repository.GetAllForOrg(It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<FakeRepositoory>(new[]
                {
                    new FakeRepositoory("root/test1", "test1", "root"),
                    new FakeRepositoory("root/test2", "test2", "root"),
                    new FakeRepositoory("root/test3", "test3", "root")
                }));

            _client
                .SetupSequence(c => c.Repository.Branch.GetAll(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ReadOnlyCollection<FakeBranch>(new[] { new FakeBranch("master") }))
                .ReturnsAsync(new ReadOnlyCollection<FakeBranch>(new[] { new FakeBranch("develop") }))
                .ReturnsAsync(new ReadOnlyCollection<FakeBranch>(new[] { new FakeBranch("aaaaa") }));

            var options = new CompareOptions { Owners = new[] { "root" } };

            // When
            var result = await _service.Execute(options, _client.Object);
            Assert.Equal(0, result);
            Assert.Empty(FakeConsole.Messages);
        }

        private sealed class FakeRepositoory : Octokit.Repository
        {
            public FakeRepositoory(string fullName, string name, string login)
            {
                FullName = fullName;
                Name = name;
                Owner = new FakeUser(login);
            }
        }

        private sealed class FakeBranch : Octokit.Branch
        {
            public FakeBranch(string name) => Name = name;
        }

        private sealed class FakeUser : Octokit.User
        {
            public FakeUser(string login) => Login = login;
        }
    }
}
