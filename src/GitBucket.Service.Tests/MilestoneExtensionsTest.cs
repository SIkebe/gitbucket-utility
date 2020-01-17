using System;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GitBucket.Service.Tests
{
    public class MilestoneExtensionsTest
    {
        [Fact]
        public void Should_Return_Formatted_Milestone()
        {
            // Given
            var milestone = new Milestone
            {
                Description = "Imprement xxx feature.",
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[] { new Issue { AssignedUserName = "user1" }, }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], [user1]", formatted);
        }

        [Fact]
        public void Should_Return_Formatted_Milestone_With_Multiple_Issues()
        {
            // Given
            var milestone = new Milestone
            {
                Description = "Imprement xxx feature.",
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[]
                {
                    new Issue { AssignedUserName = "user1" },
                    new Issue { AssignedUserName = "user3" },
                    new Issue { AssignedUserName = "user2" },
                    new Issue { AssignedUserName = "user2" },
                }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], [user1, user2, user3]", formatted);
        }

        [Fact]
        public void Should_Return_Formatted_Milestone_With_Long_Description()
        {
            // Given
            var milestone = new Milestone
            {
                Description = "Long-Long-Long-Long-Long-Long-Long-Long-Long-Long-Long-TItle",
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[] { new Issue { AssignedUserName = "user1" }, }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Long-Long-Long-Long-Long-Long-...], [user1]", formatted);
        }

        [Fact]
        public void Should_Return_Formatted_Milestone_With_New_Line()
        {
            // Given
            var milestone = new Milestone
            {
                Description = $"Imprement xxx feature.{Environment.NewLine}Next",
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[] { new Issue { AssignedUserName = "user1" }, }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature. Next], [user1]", formatted);
        }

        [Fact]
        public void Should_Return_Formatted_Milestone_With_No_Issue()
        {
            // Given
            var milestone = new Milestone
            {
                Description = "Imprement xxx feature.",
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = Array.Empty<Issue>()
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], []", formatted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_Formatted_Milestone_With_No_AssignedUserName(string assignedUserName)
        {
            // Given
            var milestone = new Milestone
            {
                Description = "Imprement xxx feature.",
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[] { new Issue { AssignedUserName = assignedUserName }, }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [Imprement xxx feature.], []", formatted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_Formatted_Milestone_With_No_Description(string description)
        {
            // Given
            var milestone = new Milestone
            {
                Description = description,
                DueDate = new DateTime(2019, 1, 15),
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[] { new Issue { AssignedUserName = "user1" }, }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [2019/01/15], [], [user1]", formatted);
        }

        [Fact]
        public void Should_Return_Formatted_Milestone_With_No_DueDate()
        {
            // Given
            var milestone = new Milestone
            {
                Description = "Imprement xxx feature.",
                DueDate = null,
                RepositoryName = "test",
                UserName = "root",
                Title = "v1.0.0",
                Issue = new[] { new Issue { AssignedUserName = "user1" }, }
            };

            // When
            var formatted = milestone.Format();

            // Then
            Assert.Equal("* [root/test], [v1.0.0], [], [Imprement xxx feature.], [user1]", formatted);
        }
    }
}
