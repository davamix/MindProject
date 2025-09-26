using System;
using System.Collections.Generic;

namespace MindProject.Desktop.Models;

public record Project(
     int Id,
     string Name,
     string Description,
     string RepoAddress,
     DateTime CreatedAt,
     DateTime UpdatedAt,
     DateTime? EndedAt,
     IReadOnlyList<Note> Notes = default!,
     IReadOnlyList<string> Commits = default!
);
