using Effectory.Questionnaire.Domain.Entities;

namespace Effectory.Questionnaire.Domain.Repositories;

public interface IQuestionsRepository
{
    Task<List<Question>> GetQuestions(
        long subjectId,
        int pageOffset,
        int pageSize,
        CancellationToken cancellationToken);

    Task<List<QuestionAnswerOption>> GetQuestionAnswerOptions(
        long questionId,
        int pageOffset,
        int pageSize,
        CancellationToken cancellationToken);
}
