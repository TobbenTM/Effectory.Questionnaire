using Effectory.Questionnaire.Domain.Support.AnswerContent;

namespace Effectory.Questionnaire.Domain.Repositories;

public interface IAnswerRepository
{
    Task AnswerQuestion(
        long questionId,
        long questionAnswerOptionId,
        IAnswerContent? content,
        long currentUserId,
        string currentUserDepartment,
        CancellationToken cancellationToken);
}
