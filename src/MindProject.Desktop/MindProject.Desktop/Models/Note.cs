using System;

namespace MindProject.Desktop.Models;
public record Note(
    int Id,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt
);