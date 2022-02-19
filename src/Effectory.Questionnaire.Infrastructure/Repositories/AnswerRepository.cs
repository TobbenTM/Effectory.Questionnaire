using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Domain.Support;
using Effectory.Questionnaire.Domain.Support.AnswerContent;
using Microsoft.EntityFrameworkCore;

namespace Effectory.Questionnaire.Infrastructure.Repositories;

public class AnswerRepository : IAnswerRepository, IAnswerStatisticsRepository
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
            OptionId = questionAnswerOptionId,
            Content = content,
            AnsweredByUserId = currentUserId,
            Department = currentUserDepartment,
        });
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AnswerStatistics>> CalculateStatistics(long questionId, CancellationToken cancellationToken)
    {
        var options = await _dbContext.QuestionAnswerOptions
            .AsNoTracking()
            .Where(a => a.QuestionId == questionId)
            .ToListAsync(cancellationToken);

        var answers = await _dbContext.Answers
            .Include(a => a.Option)
            .AsNoTracking()
            .Where(a => a.QuestionId == questionId)
            .ToListAsync(cancellationToken);

        var calculatedGroups = answers
            .Select(a => new {a.Department, Value = a.Option.DisplayOrder})
            .GroupBy(a => a.Department)
            .Select(grp => new AnswerStatistics(
                Department: grp.Key,
                Minimum: options.First(o => o.DisplayOrder == grp.Min(a => a.Value)),
                Maximum: options.First(o => o.DisplayOrder == grp.Max(a => a.Value)),
                Average: options.First(o => o.DisplayOrder == (int) Math.Round(grp.Average(a => a.Value)))));

        return calculatedGroups.ToList();
    }
}
