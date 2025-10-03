using System;
using System.Collections.Generic;

namespace MindProject.Desktop.Models;

public class Project {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RepoAddress { get; set; } = string.Empty;
    public DateTime CreatedAt{get;set;}
    public DateTime UpdatedAt{get;set;}
    public DateTime? EndedAt{get;set;}
    public IReadOnlyList<Note> Notes = default!;
    public IReadOnlyList<string> Commits = default!;
};
