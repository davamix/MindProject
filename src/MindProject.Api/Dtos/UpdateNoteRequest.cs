namespace MindProject.Api.Dtos;

public class UpdateNoteRequest {
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
}