using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Domain.Support.AnswerContent;

namespace Effectory.Questionnaire.Infrastructure.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly QuestionnaireDbContext _dbContext;

    public AnswerRepository(QuestionnaireDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AnswerQuestion(
        long questionId,
        long questionAnswerOptionId,
        IAnswerContent? content,
        long currentUserId,
        string currentUserDepartment,
        CancellationToken cancellationToken)
    {
        _dbContext.Answers.Add(new Answer
        {
            QuestionId = questionId,
            QuestionAnswerOptionId = questionAnswerOptionId,
            Content = content,
            AnsweredByUserId = currentUserId,
            Department = currentUserDepartment,
        });
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
