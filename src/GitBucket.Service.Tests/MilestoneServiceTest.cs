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
                    DueDate = new DateTime(2018, 7, 8),
                    ClosedDate = null,
                    Description = "Implement xxx feature",
                    UserName = "root"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
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

            Assert.Equal("* root/test1, v0.1.0, 2018/07/08, Implement xxx feature", console.Messages[2]);
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
                    DueDate = new DateTime(2018, 7, 1),
                    ClosedDate = new DateTime(2018, 7, 1),
                    Description = "Implement xxx feature",
                    UserName = "root"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1), IncludeClosed = true };
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

            Assert.Equal("* root/test1, v0.1.0, 2018/07/01, Implement xxx feature", console.Messages[2]);
        }

        [Fact]
        public void Should_Show_Expired_Milestones_With_Error()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 6, 30),
                    ClosedDate = null,
                    Description = "Implement xxx feature",
                    UserName = "root"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(2, console.Messages.Count);
            Assert.Empty(console.WarnMessages);
            Assert.Single(console.ErrorMessages);

            Assert.Equal("There are 1 open milestone.", console.Messages[0]);
            Assert.Equal(string.Empty, console.Messages[1]);

            Assert.Equal("* root/test1, v0.1.0, 2018/06/30, Implement xxx feature", console.ErrorMessages[0]);
        }

        [Fact]
        public void Should_Show_Milestones_To_Be_Closed_In_A_Week_With_Warn()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 7),
                    ClosedDate = null,
                    Description = "Implement xxx feature",
                    UserName = "root"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
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

            Assert.Equal("* root/test1, v0.1.0, 2018/07/07, Implement xxx feature", console.WarnMessages[0]);
        }

        [Fact]
        public void Should_Show_Milestones_With_Warn_If_DueDate_Equals_ExecutedDate()
        {
            // Given
            var mockRepository = new Mock<MilestoneRepositoryBase>(new Mock<DbContext>().Object);
            mockRepository.Setup(m => m.FindMilestones(It.IsAny<MilestoneOptions>())).Returns(new[]
            {
                new Milestone
                {
                    Title = "v0.1.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 1),
                    ClosedDate = null,
                    Description = "Implement xxx feature",
                    UserName = "root"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1) };
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

            Assert.Equal("* root/test1, v0.1.0, 2018/07/01, Implement xxx feature", console.WarnMessages[0]);
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
                    DueDate = new DateTime(2018, 6, 30),
                    ClosedDate = null,
                    Description = "Error",
                    UserName = "root"
                },
                new Milestone
                {
                    Title = "v0.2.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 1),
                    ClosedDate = new DateTime(2018, 7, 1),
                    Description = "Closed",
                    UserName = "root"
                },
                new Milestone
                {
                    Title = "v0.3.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 7),
                    ClosedDate = null,
                    Description = "Warn",
                    UserName = "root"
                },
                new Milestone
                {
                    Title = "v0.4.0",
                    RepositoryName = "test1",
                    DueDate = new DateTime(2018, 7, 8),
                    ClosedDate = null,
                    Description = "Info",
                    UserName = "root"
                }
            });

            var options = new MilestoneOptions { ExecutedDate = new DateTime(2018, 7, 1), IncludeClosed = true };
            var console = new FakeConsole();
            var service = new MilestoneService(mockRepository.Object, console);

            // When
            var result = service.ShowMilestones(options);

            // Then
            Assert.Equal(0, result);
            Assert.Equal(4, console.Messages.Count);
            Assert.Single(console.WarnMessages);
            Assert.Single(console.ErrorMessages);

            Assert.Equal("There are 4 milestones.", console.Messages[0]);
            Assert.Equal(string.Empty, console.Messages[1]);
            
            Assert.Equal("* root/test1, v0.1.0, 2018/06/30, Error", console.ErrorMessages[0]);
            Assert.Equal("* root/test1, v0.2.0, 2018/07/01, Closed", console.Messages[2]);
            Assert.Equal("* root/test1, v0.3.0, 2018/07/07, Warn", console.WarnMessages[0]);
            Assert.Equal("* root/test1, v0.4.0, 2018/07/08, Info", console.Messages[3]);
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
            Assert.Single(console.Messages);
            Assert.Empty(console.WarnMessages);
            Assert.Empty(console.ErrorMessages);

            Assert.Equal("There are no milestone.", console.Messages[0]);
        }
    }
}
