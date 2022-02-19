using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Effectory.Questionnaire.Infrastructure.Repositories;

public class QuestionsRepository : IQuestionsRepository
{
    private readonly QuestionnaireDbContext _dbContext;

    public QuestionsRepository(QuestionnaireDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Question>> GetQuestions(long subjectId, int pageOffset, int pageSize, CancellationToken cancellationToken)
        => _dbContext.Questions
            .AsNoTracking()
            .Where(q => q.SubjectId == subjectId)
            .OrderBy(q => q.DisplayOrder)
            .Skip(pageOffset * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public Task<List<QuestionAnswerOption>> GetQuestionAnswerOptions(long questionId, int pageOffset, int pageSize, CancellationToken cancellationToken)
        => _dbContext.QuestionAnswerOptions
            .AsNoTracking()
            .Where(o => o.QuestionId == questionId)
            .OrderBy(q => q.DisplayOrder)
            .Skip(pageOffset * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
}
