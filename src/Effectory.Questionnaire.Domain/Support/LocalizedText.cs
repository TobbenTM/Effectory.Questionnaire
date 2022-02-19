namespace Effectory.Questionnaire.Domain.Support;

public class LocalizedText : Dictionary<string, string>
{
    public LocalizedText()
    {
    }

    public LocalizedText(IDictionary<string, string> value)
        : base(value)
    {
    }
}
