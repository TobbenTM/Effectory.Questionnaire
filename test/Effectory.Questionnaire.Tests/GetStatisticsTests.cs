using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Domain.Support;
using Effectory.Questionnaire.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Effectory.Questionnaire.Tests;

public class GetStatisticsTests
{
    private readonly HttpClient _client;
    private readonly IServiceProvider _serviceProvider;

    public GetStatisticsTests()
    {
        var factory = new QuestionnaireWebApplicationFactory();
        _client = factory.CreateClient();
        _serviceProvider = factory.Services;
    }

    [Fact]
    public async Task GivenAnswersInDatabase_WhenCallingEndpoint_ReturnsCorrectStatistics()
    {
        // Arrange
        var expectedDepartment = "Test Department";
        using (var setupScope = _serviceProvider.CreateScope())
        {
            var dbContext = setupScope.ServiceProvider.GetRequiredService<QuestionnaireDbContext>();
            var answerRepo = setupScope.ServiceProvider.GetRequiredService<IAnswerRepository>();
            var root = await SeedSingleQuestion(dbContext);
            var question = root.Subjects.Single().Questions.Single();

            await answerRepo.AnswerQuestion(question.Id, question.Options.First().Id, null, 0, expectedDepartment,
                CancellationToken.None);
            await answerRepo.AnswerQuestion(question.Id, question.Options.First().Id, null, 0, expectedDepartment,
                CancellationToken.None);
            await answerRepo.AnswerQuestion(question.Id, question.Options.Last().Id, null, 0, expectedDepartment,
                CancellationToken.None);
        }

        // Act
        var response = await _client.GetAsync("/statistics/?questionId=1");
        var content = await response.Content.ReadAsStringAsync();
        var statistics = JsonConvert.DeserializeObject<AnswerStatistics[]>(content);

        // Assert
        var stat = statistics.SingleOrDefault();
        Assert.NotNull(stat);
        Assert.Equal(expectedDepartment, stat!.Department);
        Assert.Equal(1, stat.Minimum.Id);
        Assert.Equal(2, stat.Average.Id); // 1.5 will round up
        Assert.Equal(3, stat.Maximum.Id);
    }

    private async Task<RootQuestionnaire> SeedSingleQuestion(QuestionnaireDbContext dbContext)
    {
        var root = dbContext.Questionnaires.Add(new RootQuestionnaire
        {
            Subjects = new[]
            {
                new Subject
                {
                    Questions = new[]
                    {
                        new Question
                        {
                            Id = 1,
                            Options = new[]
                            {
                                new QuestionAnswerOption
                                {
                                    Id = 1,
                                    DisplayOrder = 0
                                },
                                new QuestionAnswerOption
                                {
                                    Id = 2,
                                    DisplayOrder = 1
                                },
                                new QuestionAnswerOption
                                {
                                    Id = 3,
                                    DisplayOrder = 2
                                },
                            }
                        }
                    }
                }
            }
        });
        await dbContext.SaveChangesAsync();
        return root.Entity;
    }
}
