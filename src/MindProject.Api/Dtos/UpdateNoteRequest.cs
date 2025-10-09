namespace MindProject.Api.Dtos;

public class UpdateNoteRequest {
    public Guid NoteId { get; set; }
    public Guid ProjectId { get; set; }
    public string Content { get; set; } = null!;
}