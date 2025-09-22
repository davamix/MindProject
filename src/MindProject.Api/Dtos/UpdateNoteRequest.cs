namespace MindProject.Api.Dtos;

public class UpdateNoteRequest {
    public int Id { get; set; }
    public string Content { get; set; } = null!;
}