using System.ComponentModel.DataAnnotations;
using Effectory.Questionnaire.Domain.Support.AnswerContent;

namespace Effectory.Questionnaire.API.Models;

public record AnswerRequest(
    [Required] long OptionId,
    IAnswerContent? Content,
    [Required] long UserId,
    [Required] string Department);
