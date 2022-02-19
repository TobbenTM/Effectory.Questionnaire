using System;
using System.Threading;
using System.Threading.Tasks;
using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Effectory.Questionnaire.Infrastructure.Tests.Repositories;

public class GetQuestionAnswerOptionsTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly DbContextOptions<QuestionnaireDbContext> _dbOpts;
    private readonly Func<DbContextOptions<QuestionnaireDbContext>, QuestionnaireDbContext> _createDatabase;
    private readonly IQuestionsRepository _repo;

    public GetQuestionAnswerOptionsTests(InMemoryDatabaseFixture fixture)
    {
        _dbOpts = fixture.CreateDbContextOptions();
        _createDatabase = fixture.CreateDatabase;
        _repo = new QuestionsRepository(_createDatabase(_dbOpts));
    }

    [Fact]
    public async Task GivenNoQuestionsInDb_WhenGettingAnyOptions_ReturnsEmptyCollection()
    {
        // Arrange
        // no-op

        // Act
        var result = await _repo.GetQuestionAnswerOptions(123, 0, 1, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenQuestionsInDb_WhenGettingAllOptions_ReturnsAllInDisplayOrder()
    {
        // Arrange
        const int expectedQuestionId = 10;
        await using (var dbContext = _createDatabase(_dbOpts))
        {
            dbContext.Questionnaires.Add(new RootQuestionnaire { Id = 1, Subjects = new []
            {
                new Subject
                {
                    Id = 2,
                    Questions = new []
                    {
                        new Question
                        {
                            Id = expectedQuestionId,
                            DisplayOrder = 0,
                            Options = new []
                            {
                                new QuestionAnswerOption
                                {
                                    OptionId = 100,
                                },
                                new QuestionAnswerOption
                                {
                                    OptionId = 200,
                                },
                            }
                        }
                    }
                }
            }});
            await dbContext.SaveChangesAsync();
        }

        // Act
        var results = await _repo.GetQuestionAnswerOptions(expectedQuestionId, 0, int.MaxValue, CancellationToken.None);

        // Assert
        Assert.Collection(results,
            o => Assert.Equal(100, o.OptionId),
            o => Assert.Equal(200, o.OptionId));
    }

    [Fact]
    public async Task GivenQuestionsInDb_WhenGettingSecondPage_SkipsFirstPage()
    {
        // Arrange
        const int expectedQuestionId = 10;
        await using (var dbContext = _createDatabase(_dbOpts))
        {
            dbContext.Questionnaires.Add(new RootQuestionnaire { Id = 1, Subjects = new []
            {
                new Subject
                {
                    Id = 2,
                    Questions = new []
                    {
                        new Question
                        {
                            Id = expectedQuestionId,
                            DisplayOrder = 0,
                            Options = new []
                            {
                                new QuestionAnswerOption
                                {
                                    OptionId = 100,
                                },
                                new QuestionAnswerOption
                                {
                                    OptionId = 200,
                                },
                            }
                        }
                    }
                }
            }});
            await dbContext.SaveChangesAsync();
        }

        // Act
        var results = await _repo.GetQuestionAnswerOptions(expectedQuestionId, 1, 1, CancellationToken.None);

        // Assert
        Assert.Collection(results, o => Assert.Equal(200, o.OptionId));
    }
}
