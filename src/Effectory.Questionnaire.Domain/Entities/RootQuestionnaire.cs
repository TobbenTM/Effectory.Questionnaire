namespace Effectory.Questionnaire.Domain.Entities;

public class RootQuestionnaire
{
    public long Id { get; init; }

    public IEnumerable<Subject> Subjects { get; init; } = new List<Subject>();
}
