namespace MindProject.Api.Dtos;

public record NoteUpdatedResponse(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt);