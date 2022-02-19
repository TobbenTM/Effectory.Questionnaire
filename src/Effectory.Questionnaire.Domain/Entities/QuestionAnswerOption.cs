using Effectory.Questionnaire.Domain.Support;

namespace Effectory.Questionnaire.Domain.Entities;

public class QuestionAnswerOption
{
    public long OptionId { get; init; }

    public long QuestionId { get; init; }
    public Question Question { get; init; } = null!;

    public int DisplayOrder { get; init; }

    public LocalizedText? Text { get; init; }
}
