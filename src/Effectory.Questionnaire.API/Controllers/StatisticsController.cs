using System.Net;
using Effectory.Questionnaire.API.Models;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Domain.Support;
using Microsoft.AspNetCore.Mvc;

namespace Effectory.Questionnaire.API.Controllers;

/// <summary>
/// All endpoints related to getting statistics for answers on questions
/// </summary>
[ApiController]
[Route("statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IAnswerStatisticsRepository _repository;

    public StatisticsController(IAnswerStatisticsRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Calculated the current min/max/average for a given question across departments
    /// </summary>
    /// <param name="questionId">Id of the question to calculate for</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of calculations grouped on department</returns>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(PagedResponse<AnswerStatistics>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<AnswerStatistics>>> CalculateStatistics(
        [FromQuery] long questionId,
        CancellationToken cancellationToken = default)
    {
        var statistics = await _repository.CalculateStatistics(questionId, cancellationToken);

        if (!statistics.Any())
        {
            return NoContent();
        }

        return statistics;
    }
}
