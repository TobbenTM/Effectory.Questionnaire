using Effectory.Questionnaire.Domain.Support;

namespace Effectory.Questionnaire.Domain.Entities;

public class Question
{
    public long Id { get; init; }

    public long SubjectId { get; init; }
    public Subject Subject { get; init; } = null!;

    public int DisplayOrder { get; init; }

    public LocalizedText Text { get; init; } = new();

    public IEnumerable<QuestionAnswerOption> Options { get; init; } = new List<QuestionAnswerOption>();
}
