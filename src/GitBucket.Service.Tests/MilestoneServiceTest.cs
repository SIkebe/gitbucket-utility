using System;
using System.Linq;
using System.Text;
using GitBucket.Core;
using GitBucket.Core.Models;
using GitBucket.Data.Repositories;
using GitBucket.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GitBucket.Service.Tests
{
    public class MilestoneServiceTest
    {
        [Fact]
        public void Should_Show_Open_Milestone()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 29),
                    ClosedDate = null,
                    Description = "Implement xxx feature"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 29) };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(3, console.Messages.Count);
            Assert.Empty(console.WarnMessages);
            Assert.Empty(console.ErrorMessages);

            Assert.Equal("There are 1 open milestone.", console.Messages[0]);
            Assert.Equal(string.Empty, console.Messages[1]);

            Assert.Equal("* test1, v0.1.0, 2018/07/29, Implement xxx feature", console.Messages[2]);
        }
        
        [Fact]
        public void Should_Include_Closed_Milestone_If_Specified()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 29),
                    ClosedDate = new DateTime(2018, 7, 29),
                    Description = "Implement xxx feature"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 29), IncludeClosed = true };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(3, console.Messages.Count);
            Assert.Empty(console.WarnMessages);
            Assert.Empty(console.ErrorMessages);

            Assert.Equal("There are 1 milestone.", console.Messages[0]);
            Assert.Equal(string.Empty, console.Messages[1]);

            Assert.Equal("* test1, v0.1.0, 2018/07/29, Implement xxx feature", console.Messages[2]);
        }

        [Fact]
        public void Should_Show_Expired_Milestones_With_Warning()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 28),
                    ClosedDate = null,
                    Description = "Implement xxx feature"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 29) };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(2, console.Messages.Count);
            Assert.Single(console.WarnMessages);
            Assert.Empty(console.ErrorMessages);

            Assert.Equal("There are 1 open milestone.", console.Messages[0]);
            Assert.Equal(string.Empty, console.Messages[1]);

            Assert.Equal("* test1, v0.1.0, 2018/07/28, Implement xxx feature", console.WarnMessages[0]);
        }

        [Fact]
        public void Should_Show_Multiple_Milestones()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = null,
                    ClosedDate = null,
                    Description = "Implement xxx feature"
                },
                new Milestone
                {
                    Title = "v0.2.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 28),
                    ClosedDate = null
                },
                new Milestone
                {
                    Title = "v1.0.0",
                    RepositoryName = "test2",
                    DueDate = new DateTime(2018, 7, 30),
                    ClosedDate = null,
                    Description = "Bugfix for #123"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 29) };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(4, console.Messages.Count);
            Assert.Single(console.WarnMessages);
            Assert.Empty(console.ErrorMessages);

            Assert.Equal("There are 3 open milestones.", console.Messages[0]);
            Assert.Equal(string.Empty, console.Messages[1]);

            Assert.Equal("* test1, v0.1.0, , Implement xxx feature", console.Messages[2]);
            Assert.Equal("* test1, v0.2.0, 2018/07/28, ", console.WarnMessages[0]);
            Assert.Equal("* test2, v1.0.0, 2018/07/30, Bugfix for #123", console.Messages[3]);
        }

        [Fact]
        public void Should_Return_If_No_Milestone()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(Array.Empty<Milestone>());
            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 29) };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(1, console.Messages.Count);
            Assert.Empty(console.WarnMessages);
            Assert.Empty(console.ErrorMessages);

            Assert.Equal("There are no milestone.", console.Messages[0]);
        }
    }
}
