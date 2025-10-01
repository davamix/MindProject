namespace MindProject.Api.Dtos;

public record NoteCreatedResponse(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt);