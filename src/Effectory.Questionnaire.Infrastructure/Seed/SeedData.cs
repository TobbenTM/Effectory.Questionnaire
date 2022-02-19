using System.Reflection;
using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Support;
using Newtonsoft.Json;

namespace Effectory.Questionnaire.Infrastructure.Seed;

internal class SeedData
{
    private readonly SampleRootQuestionnaire _root;

    private SeedData(SampleRootQuestionnaire seed)
    {
        _root = seed;
    }

    public static async Task<SeedData> Load()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly
            .GetManifestResourceNames()
            .Single(res => res.EndsWith("questionnaire.json"));
        await using var stream = assembly.GetManifestResourceStream(resourcePath);
        using var reader = new StreamReader(stream!);
        var seedData = await reader.ReadToEndAsync();
        var root = JsonConvert.DeserializeObject<SampleRootQuestionnaire>(seedData);
        return new SeedData(root);
    }

    public RootQuestionnaire Root =>
        new()
        {
            Id = _root.QuestionnaireId
        };

    public IEnumerable<Subject> Subjects =>
        _root.QuestionnaireItems.Select(subject => new Subject
        {
            Id = subject.SubjectId,
            DisplayOrder = subject.OrderNumber,
            Text = new LocalizedText(subject.Texts),
        });

    public IEnumerable<Question> Questions =>
        _root.QuestionnaireItems
            .SelectMany(subject => subject.QuestionnaireItems)
            .Select(question => new Question
            {
                Id = question.QuestionId,
                SubjectId = question.SubjectId,
                DisplayOrder = question.OrderNumber,
                Text = new LocalizedText(question.Texts),
            });

    public IEnumerable<QuestionAnswerOption> QuestionAnswerOptions =>
        _root.QuestionnaireItems
            .SelectMany(subject => subject.QuestionnaireItems.SelectMany(question => question.QuestionnaireItems))
            .Select(option => new QuestionAnswerOption
            {
                Id = option.AnswerId ?? -1, // A bit hacky this, can't have nullable key columns
                QuestionId = option.QuestionId,
                DisplayOrder = option.OrderNumber,
                Text = option.Texts == null ? null : new LocalizedText(option.Texts),
            });

    private record SampleRootQuestionnaire(
        long QuestionnaireId,
        SampleSubject[] QuestionnaireItems);

    private record SampleSubject(
        long SubjectId,
        int OrderNumber,
        Dictionary<string, string> Texts,
        SampleQuestion[] QuestionnaireItems);

    private record SampleQuestion(
        long QuestionId,
        long SubjectId,
        int OrderNumber,
        Dictionary<string, string> Texts,
        SampleAnswerOption[] QuestionnaireItems);

    private record SampleAnswerOption(
        long? AnswerId,
        long QuestionId,
        int OrderNumber,
        Dictionary<string, string>? Texts);
}
