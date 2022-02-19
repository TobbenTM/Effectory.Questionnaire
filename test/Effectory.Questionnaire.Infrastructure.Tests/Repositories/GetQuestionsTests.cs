using System;
using System.Threading;
using System.Threading.Tasks;
using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Effectory.Questionnaire.Infrastructure.Tests.Repositories;

public class GetQuestionsTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly DbContextOptions<QuestionnaireDbContext> _dbOpts;
    private readonly Func<DbContextOptions<QuestionnaireDbContext>, QuestionnaireDbContext> _createDatabase;
    private readonly IQuestionsRepository _repo;

    public GetQuestionsTests(InMemoryDatabaseFixture fixture)
    {
        _dbOpts = fixture.CreateDbContextOptions();
        _createDatabase = fixture.CreateDatabase;
        _repo = new QuestionsRepository(_createDatabase(_dbOpts));
    }

    [Fact]
    public async Task GivenNoQuestionsInDb_WhenGettingAnyQuestions_ReturnsEmptyCollection()
    {
        // Arrange
        // no-op

        // Act
        var result = await _repo.GetQuestions(123, 0, 1, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenQuestionsInDb_WhenGettingAllQuestions_ReturnsAllInDisplayOrder()
    {
        // Arrange
        const int expectedSubjectId = 2;
        await using (var dbContext = _createDatabase(_dbOpts))
        {
            dbContext.Questionnaires.Add(new RootQuestionnaire { Id = 1, Subjects = new []
            {
                new Subject
                {
                    Id = expectedSubjectId,
                    Questions = new []
                    {
                        new Question
                        {
                            Id = 10,
                            DisplayOrder = 0,
                        },
                        new Question
                        {
                            Id = 20,
                            DisplayOrder = 1,
                        }
                    }
                }
            }});
            await dbContext.SaveChangesAsync();
        }

        // Act
        var results = await _repo.GetQuestions(expectedSubjectId, 0, int.MaxValue, CancellationToken.None);

        // Assert
        Assert.Collection(results,
            q => Assert.Equal(10, q.Id),
            q => Assert.Equal(20, q.Id));
    }

    [Fact]
    public async Task GivenQuestionsInDb_WhenGettingSecondPage_SkipsFirstPage()
    {
        // Arrange
        const int expectedSubjectId = 2;
        await using (var dbContext = _createDatabase(_dbOpts))
        {
            dbContext.Questionnaires.Add(new RootQuestionnaire { Id = 1, Subjects = new []
            {
                new Subject
                {
                    Id = expectedSubjectId,
                    Questions = new []
                    {
                        new Question
                        {
                            Id = 10,
                            DisplayOrder = 0,
                        },
                        new Question
                        {
                            Id = 20,
                            DisplayOrder = 1,
                        }
                    }
                }
            }});
            await dbContext.SaveChangesAsync();
        }

        // Act
        var results = await _repo.GetQuestions(expectedSubjectId, 1, 1, CancellationToken.None);

        // Assert
        Assert.Collection(results, q => Assert.Equal(20, q.Id));
    }
}
