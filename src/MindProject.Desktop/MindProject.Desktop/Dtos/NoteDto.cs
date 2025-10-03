using System;

namespace MindProject.Desktop.Dtos;

public class NoteDto {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
