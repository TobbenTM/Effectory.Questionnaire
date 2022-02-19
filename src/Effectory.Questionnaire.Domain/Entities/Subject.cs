using Effectory.Questionnaire.Domain.Support;

namespace Effectory.Questionnaire.Domain.Entities;

public class Subject
{
    public long Id { get; init; }

    public int DisplayOrder { get; init; }

    public LocalizedText Text { get; init; } = new();

    public ICollection<Question> Questions { get; init; } = new List<Question>();

    public ICollection<RootQuestionnaire> Questionnaires { get; init; } = new List<RootQuestionnaire>();
}
