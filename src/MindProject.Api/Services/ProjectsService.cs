using MindProject.Api.Data;
using MindProject.Api.Dtos;
using MindProject.Api.Models;

namespace MindProject.Api.Services;

public interface IProjectsService {
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(Guid id);
    Task<ProjectCreatedResponse> AddProjectAsync(CreateNewProjectRequest project);
    Task<ProjectUpdatedResponse> UpdateProjectAsync(UpdateProjectRequest project);
    Task<NoteCreatedResponse> AddNoteAsync(CreateNewNoteRequest note);
    Task<NoteUpdatedResponse> UpdateNoteAsync(UpdateNoteRequest note);
    Task DeleteNoteAsync(Guid noteId);
}

public class ProjectsService : IProjectsService {
    private readonly IDatabaseProvider _databaseProvider;

    public ProjectsService(IDatabaseProvider databaseProvider) {
        _databaseProvider = databaseProvider;
    }

    public async Task<List<Project>> GetAllProjectsAsync() {
        return await _databaseProvider.GetProjectsAsync();
    }

    public async Task<Project> GetProjectByIdAsync(Guid id) {
        return await _databaseProvider.GetProjectAsync(id);
    }

    public async Task<ProjectCreatedResponse> AddProjectAsync(CreateNewProjectRequest project) {
        var newProject = new Project {
            Id = Guid.NewGuid(),
            Name = project.Name,
            Description = project.Description,
            RepoAddress = project.RepoAddress
        };

        await _databaseProvider.AddProjectAsync(newProject);

        return new ProjectCreatedResponse(
            newProject.Id,
            newProject.Name,
            newProject.Description,
            newProject.RepoAddress,
            newProject.CreatedAt,
            newProject.UpdatedAt,
            newProject.EndedAt);
    }

    public async Task<ProjectUpdatedResponse> UpdateProjectAsync(UpdateProjectRequest project) {
        var updateProject = await _databaseProvider.GetProjectAsync(project.Id);

        updateProject.Name = project.Name;
        updateProject.Description = project.Description;
        updateProject.RepoAddress = project.RepoAddress;
        updateProject.UpdatedAt = project.UpdatedAt;
        updateProject.EndedAt = project.EndedAt;

        await _databaseProvider.UpdateProjectAsync(updateProject);

        return new ProjectUpdatedResponse(
            updateProject.Id,
            updateProject.Name,
            updateProject.Description,
            updateProject.RepoAddress,
            updateProject.CreatedAt,
            updateProject.UpdatedAt,
            updateProject.EndedAt);
    }

    public async Task<NoteCreatedResponse> AddNoteAsync(CreateNewNoteRequest note) {
        var newNote = new Note {
            Id = Guid.NewGuid(),
            Content = note.Content
        };

        await _databaseProvider.AddNoteAsync(note.ProjectId, newNote);

        return new NoteCreatedResponse(
            newNote.Id,
            newNote.Content,
            newNote.CreatedAt,
            newNote.UpdatedAt);
    }

    public async Task<NoteUpdatedResponse> UpdateNoteAsync(UpdateNoteRequest note) {
        var updateNote = new Note {
            Id = note.Id,
            Content = note.Content,
            UpdatedAt = DateTime.UtcNow
        };

        await _databaseProvider.UpdateNoteAsync(updateNote);

        return new NoteUpdatedResponse(
            updateNote.Id,
            updateNote.Content,
            updateNote.CreatedAt,
            updateNote.UpdatedAt);
    }
    
    public async Task DeleteNoteAsync(Guid noteId) {
        await _databaseProvider.DeleteNoteAsync(noteId);
    }
}