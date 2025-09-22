namespace MindProject.Api.Models;

public class Project {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<Note> Notes { get; set; } = new();
    public string RepoAddress { get; set; } = null!;
    public List<string> Commits { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
}