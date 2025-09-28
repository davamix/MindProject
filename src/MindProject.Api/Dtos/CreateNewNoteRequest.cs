namespace MindProject.Api.Dtos;

public class CreateNewNoteRequest {
    public int ProjectId{ get; set; }
    public string Content { get; set; } = null!;
}
