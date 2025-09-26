using System.Collections.Generic;

namespace MindProject.Desktop.Dtos;

public class ProjectDetailDto : ProjectInfoDto {
    public List<NoteDto> Notes { get; set; } = new();
    public List<string> Commits { get; set; } = new();
}
