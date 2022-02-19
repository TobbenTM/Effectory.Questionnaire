namespace Effectory.Questionnaire.API.Models;

public record PagedResponse<TItem>(
    int CurrentPage,
    int PageSize,
    string? Next,
    IEnumerable<TItem> Items);
