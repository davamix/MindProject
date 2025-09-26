using System;

namespace MindProject.Desktop.Dtos;

public class ProjectInfoDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string RepoAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}
