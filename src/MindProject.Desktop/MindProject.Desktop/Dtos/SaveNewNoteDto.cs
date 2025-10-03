using System;

namespace MindProject.Desktop.Dtos;

class SaveNewNoteDto {
    public Guid ProjectId { get; set; }
    public string Content { get; set; }
}
