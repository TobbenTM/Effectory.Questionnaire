using Effectory.Questionnaire.Domain.Support.AnswerContent;

namespace Effectory.Questionnaire.Domain.Entities;

public class Answer
{
    public long Id { get; init; }

    public long QuestionId { get; init; }
    public Question Question { get; init; } = null!;

    public long OptionId { get; init; }
    public QuestionAnswerOption Option { get; init; } = null!;

    public IAnswerContent? Content { get; init; }

    public long AnsweredByUserId { get; init; }
    public string Department { get; init; } = null!;
}
