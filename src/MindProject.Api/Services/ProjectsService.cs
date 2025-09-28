using MindProject.Api.Data;
using MindProject.Api.Dtos;
using MindProject.Api.Models;

namespace MindProject.Api.Services;

public interface IProjectsService {
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(int id);
    Task AddProjectAsync(CreateNewProjectRequest project);
    Task UpdateProjectAsync(UpdateProjectRequest project);
    Task AddNoteAsync(CreateNewNoteRequest note);
    Task UpdateNoteAsync(int noteId, UpdateNoteRequest note);
    Task DeleteNoteAsync(int noteId);
}

public class ProjectsService : IProjectsService {
    private readonly IDatabaseProvider _databaseProvider;

    public ProjectsService(IDatabaseProvider databaseProvider) {
        _databaseProvider = databaseProvider;
    }

    public async Task<List<Project>> GetAllProjectsAsync() {
        return await _databaseProvider.GetProjectsAsync();
    }

    public async Task<Project> GetProjectByIdAsync(int id) {
        return await _databaseProvider.GetProjectAsync(id);
    }

    public async Task AddProjectAsync(CreateNewProjectRequest project) {
        var newProject = new Project {
            Name = project.Name,
            Description = project.Description,
            RepoAddress = project.RepoAddress
        };

        await _databaseProvider.AddProjectAsync(newProject);
    }

    public async Task UpdateProjectAsync(UpdateProjectRequest project) {
        var updateProject = await _databaseProvider.GetProjectAsync(project.Id);

        updateProject.Name = project.Name;
        updateProject.Description = project.Description;
        updateProject.RepoAddress = project.RepoAddress;
        updateProject.UpdatedAt = project.UpdatedAt;
        updateProject.EndedAt = project.EndedAt;

        await _databaseProvider.UpdateProjectAsync(updateProject);
    }

    public async Task AddNoteAsync(CreateNewNoteRequest note) {
        var newNote = new Note {
            Content = note.Content
        };

        await _databaseProvider.AddNoteAsync(note.ProjectId, newNote);
    }

    public async Task UpdateNoteAsync(int noteId, UpdateNoteRequest note) {
        var updateNote = await _databaseProvider.GetNoteAsync(noteId);
        updateNote.Content = note.Content;
        updateNote.UpdatedAt = DateTime.UtcNow;

        await _databaseProvider.UpdateNoteAsync(updateNote);
    }
    
    public async Task DeleteNoteAsync(int noteId) {
        await _databaseProvider.DeleteNoteAsync(noteId);
    }
}