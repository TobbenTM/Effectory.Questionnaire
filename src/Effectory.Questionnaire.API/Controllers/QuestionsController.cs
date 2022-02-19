using System.Net;
using Effectory.Questionnaire.API.Models;
using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Effectory.Questionnaire.API.Controllers;

/// <summary>
/// All endpoints related to getting questions and answer options for questions
/// </summary>
[ApiController]
[Route("questions")]
public class QuestionsController : ControllerBase
{
    // Nit: might not be totally best practice to expose
    // repositories in controllers directly, but this
    // very small application has no business logic
    // yet, so let's be pragmatic.
    //
    // Another big issue in this controller is that
    // we're using domain models as returned view models
    // directly. This makes it very hard to change our
    // internal models, and ideally we'd have dedicated
    // view models with mappings here.
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IAnswerRepository _answerRepository;

    public QuestionsController(IQuestionsRepository questionsRepository, IAnswerRepository answerRepository)
    {
        _questionsRepository = questionsRepository;
        _answerRepository = answerRepository;
    }

    /// <summary>
    /// Returns a paged response of questions for any given subject
    /// </summary>
    /// <param name="subjectId">Id of the subject to get questions for</param>
    /// <param name="currentPage">Current page of questions</param>
    /// <param name="pageSize">Amount of questions to return per call</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged collection of questions</returns>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(PagedResponse<Question>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResponse<Question>>> GetQuestions(
        [FromQuery] long subjectId,
        [FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 1,
        CancellationToken cancellationToken = default)
    {
        var questions = await _questionsRepository.GetQuestions(subjectId, currentPage - 1, pageSize, cancellationToken);

        if (!questions.Any())
        {
            return NoContent();
        }

        return new PagedResponse<Question>(
            currentPage,
            pageSize,
            Next: Url.Action(nameof(GetQuestions), "Questions", new
            {
                subjectId,
                currentPage = currentPage + 1,
                pageSize,
            }),
            Items: questions);
    }

    /// <summary>
    /// Returns a paged response of answer options for any given question
    /// </summary>
    /// <param name="questionId">Id of the question to get options for</param>
    /// <param name="currentPage">Current page of options</param>
    /// <param name="pageSize">Amount of options to return per call</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged collection of answer options</returns>
    [HttpGet("{questionId:long}/options")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(PagedResponse<QuestionAnswerOption>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResponse<QuestionAnswerOption>>> GetQuestionsAnswerOptions(
        long questionId,
        [FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 1,
        CancellationToken cancellationToken = default)
    {
        var options =
            await _questionsRepository.GetQuestionAnswerOptions(questionId, currentPage - 1, pageSize, cancellationToken);

        if (!options.Any())
        {
            return NoContent();
        }

        return new PagedResponse<QuestionAnswerOption>(
            currentPage,
            pageSize,
            Next: Url.Action(nameof(GetQuestionsAnswerOptions), "Questions", new
            {
                questionId,
                currentPage = currentPage + 1,
                pageSize,
            }),
            Items: options);
    }

    /// <summary>
    /// Creates a new answer to a question
    /// </summary>
    /// <param name="questionId">Id of the question to answer</param>
    /// <param name="answer">Answer parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Whether or not the answer has been accepted</returns>
    [HttpPost("{questionId:long}/answer")]
    [ProducesResponseType((int) HttpStatusCode.Created)]
    public async Task<IActionResult> AnswerQuestion(
        long questionId,
        [FromBody] AnswerRequest answer,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        await _answerRepository.AnswerQuestion(
            questionId,
            answer.OptionId,
            answer.Content,
            answer.UserId,
            answer.Department, cancellationToken);

        return StatusCode((int) HttpStatusCode.Created); // Maybe you're not supposed to return created without a location?
    }
}
