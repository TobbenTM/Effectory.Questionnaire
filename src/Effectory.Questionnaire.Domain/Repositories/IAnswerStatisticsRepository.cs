using Effectory.Questionnaire.Domain.Support;

namespace Effectory.Questionnaire.Domain.Repositories;

public interface IAnswerStatisticsRepository
{
    Task<List<AnswerStatistics>> CalculateStatistics(
        long questionId,
        CancellationToken cancellationToken);
}
