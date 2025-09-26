using MindProject.Desktop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindProject.Desktop.Models.Mappers;
public static class ProjectModelExtensions {
    public static Project ToProject(this ProjectInfoDto dto) {
        return new Project(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.RepoAddress,
            dto.CreatedAt,
            dto.UpdatedAt,
            dto.EndedAt
        );
    }

    public static Project ToProjectDetails(this ProjectDetailDto dto) {
        return new Project(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.RepoAddress,
            dto.CreatedAt,
            dto.UpdatedAt,
            dto.EndedAt,
            dto.Notes.Select(n => n.ToNote()).ToList(),
            dto.Commits
        );
    }

    public static Note ToNote(this NoteDto dto) {
        return new Note(
            dto.Id,
            dto.Content,
            dto.CreatedAt,
            dto.UpdatedAt
        );
    }
}
