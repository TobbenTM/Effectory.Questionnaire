using Effectory.Questionnaire.Domain.Entities;

namespace Effectory.Questionnaire.Domain.Support;

public record AnswerStatistics(
    QuestionAnswerOption Average,
    QuestionAnswerOption Minimum,
    QuestionAnswerOption Maximum,
    string Department);
