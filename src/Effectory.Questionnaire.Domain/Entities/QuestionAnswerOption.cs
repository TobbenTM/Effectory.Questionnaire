using Effectory.Questionnaire.Domain.Support;

namespace Effectory.Questionnaire.Domain.Entities;

public class QuestionAnswerOption
{
    public long Id { get; init; }

    public long QuestionId { get; init; }
    public Question Question { get; init; } = null!;

    public int DisplayOrder { get; init; }

    public LocalizedText? Text { get; init; }

    public ICollection<Answer> Answers { get; init; } = new List<Answer>();
}
