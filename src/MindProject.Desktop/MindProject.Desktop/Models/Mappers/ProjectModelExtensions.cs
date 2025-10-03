using MindProject.Desktop.Dtos;
using System.Linq;

namespace MindProject.Desktop.Models.Mappers;
public static class ProjectModelExtensions {
    public static Project ToProject(this ProjectInfoDto dto) {
        return new Project() {
            Id = dto.Id,
            Name =dto.Name,
            Description = dto.Description,
            RepoAddress = dto.RepoAddress,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            EndedAt = dto.EndedAt
        };
    }

    public static Project ToProjectDetails(this ProjectDetailDto dto) {
        return new Project() {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            RepoAddress = dto.RepoAddress,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            EndedAt = dto.EndedAt,
            Notes = dto.Notes.Select(n => n.ToNote()).ToList(),
            Commits = dto.Commits
        };
    }

    public static Note ToNote(this NoteDto dto) {
        return new Note() {
            Id = dto.Id,
            Content = dto.Content,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
