namespace MindProject.Api.Dtos;

public class CreateNewNoteRequest {
    public Guid ProjectId{ get; set; }
    public string Content { get; set; } = null!;
}
