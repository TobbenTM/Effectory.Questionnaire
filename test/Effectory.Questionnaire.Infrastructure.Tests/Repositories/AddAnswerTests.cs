using System;
using System.Threading;
using System.Threading.Tasks;
using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Domain.Support.AnswerContent;
using Effectory.Questionnaire.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Effectory.Questionnaire.Infrastructure.Tests.Repositories;

public class AddAnswerTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly DbContextOptions<QuestionnaireDbContext> _dbOpts;
    private readonly Func<DbContextOptions<QuestionnaireDbContext>, QuestionnaireDbContext> _createDatabase;
    private readonly IAnswerRepository _repo;

    public AddAnswerTests(InMemoryDatabaseFixture fixture)
    {
        _dbOpts = fixture.CreateDbContextOptions();
        _createDatabase = fixture.CreateDatabase;
        _repo = new AnswerRepository(_createDatabase(_dbOpts));
    }

    [Fact]
    public async Task GivenParentEntitiesInDb_WhenAddingAnswerWithText_AddsToDatabase()
    {
        // Arrange
        const int expectedQuestionId = 10;
        const int expectedQuestionAnswerOptionId = 20;
        const string expectedAnswerContent = "Hello world!";
        const int expectedUserId = 42;
        const string expectedDepartment = "Sales";

        await using (var dbContext = _createDatabase(_dbOpts))
        {
            dbContext.Questionnaires.Add(new RootQuestionnaire
            {
                Id = 1, Subjects = new[]
                {
                    new Subject
                    {
                        Id = 2,
                        Questions = new[]
                        {
                            new Question
                            {
                                Id = expectedQuestionId,
                                DisplayOrder = 0,
                                Options = new[]
                                {
                                    new QuestionAnswerOption
                                    {
                                        Id = expectedQuestionAnswerOptionId,
                                    },
                                }
                            }
                        }
                    }
                }
            });
        }

        // Act
        await _repo.AnswerQuestion(expectedQuestionId, expectedQuestionAnswerOptionId, new FreeTextAnswerContent
        {
            Text = expectedAnswerContent
        }, expectedUserId, expectedDepartment, CancellationToken.None);

        // Assert
        await using var assertDbContext = _createDatabase(_dbOpts);
        var answer = await assertDbContext.Answers.SingleAsync();
        Assert.Equal(expectedUserId, answer.AnsweredByUserId);
        Assert.Equal(expectedDepartment, answer.Department);
        var content = answer.Content as FreeTextAnswerContent;
        Assert.Equal(expectedAnswerContent, content?.Text);
    }
}
