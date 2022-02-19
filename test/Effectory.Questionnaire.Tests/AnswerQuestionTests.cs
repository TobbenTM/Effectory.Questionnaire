using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Effectory.Questionnaire.API.Models;
using Effectory.Questionnaire.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Effectory.Questionnaire.Tests;

public class AnswerQuestionTests
{

    private readonly HttpClient _client;
    private readonly IServiceProvider _serviceProvider;

    public AnswerQuestionTests()
    {
        var factory = new QuestionnaireWebApplicationFactory();
        _client = factory.CreateClient();
        _serviceProvider = factory.Services;
    }

    [Fact]
    public async Task GivenInvalidInput_WhenAnsweringQuestion_ReturnsBadRequest()
    {
        // Arrange
        var body = new AnswerRequest(1, null, 0, null!);

        // Act
        var response = await _client.PostAsJsonAsync("/questions/1/answer", body);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Small aside; the following test would fail if using an actual relational DB.
    [Fact]
    public async Task GivenValidInput_WhenAnsweringQuestion_StoresQuestion()
    {
        // Arrange
        var body = new AnswerRequest(1, null, 42, "Test Department");

        // Act
        var response = await _client.PostAsJsonAsync("/questions/1/answer", body);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var assertScope = _serviceProvider.CreateScope();
        var dbContext = assertScope.ServiceProvider.GetRequiredService<QuestionnaireDbContext>();
        var answer = await dbContext.Answers.SingleOrDefaultAsync();
        Assert.NotNull(answer);
        Assert.Equal(body.Department, answer!.Department);
    }
}
