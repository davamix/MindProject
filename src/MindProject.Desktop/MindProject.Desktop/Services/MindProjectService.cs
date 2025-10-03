using MindProject.Desktop.Dtos;
using MindProject.Desktop.Models;
using MindProject.Desktop.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Vpn;

namespace MindProject.Desktop.Services;

public interface IMindProjectService {
    Task<List<Project>> GetProjects();
    Task<Project> GetProject(Guid projectId);
    Task<Project> SaveProject(Project project);
    Task<Note> SaveNote(Note note, Guid projectId);
    Task DeleteNote(Guid noteId);
}

public class MindProjectService : IMindProjectService {

    public async Task<List<Project>> GetProjects() {
        using (var client = new HttpClient()) {
            var response = client.GetAsync(new Uri("http://localhost:8080/api/v1/projects")).Result;

            if (response.IsSuccessStatusCode) {
                var projectsResponse = await response.Content.ReadFromJsonAsync<List<ProjectInfoDto>>();

                return projectsResponse == null
                    ? Enumerable.Empty<Project>().ToList()
                    : projectsResponse.Select(x => x.ToProject()).ToList();
            }

            throw new Exception("Failed to fetch projects");
        }
    }

    public async Task<Project> GetProject(Guid id) {
        using (var client = new HttpClient()) {
            var response = client.GetAsync(new Uri($"http://localhost:8080/api/v1/projects/{id}")).Result;

            if (response.IsSuccessStatusCode) {
                var projectResponse = await response.Content.ReadFromJsonAsync<ProjectDetailDto>();

                return projectResponse == null
                    ? throw new Exception("Project not found")
                    : projectResponse.ToProjectDetails();
            }

            throw new Exception("Failed to fetch project details");
        }
    }

    public async Task<Project> SaveProject(Project project) {
        if (project.Id == Guid.Empty) {
            return await AddProject(project);
        }

        return await UpdateProject(project);
    }

    private async Task<Project> AddProject(Project project) {
        var dto = new SaveProjectDto {
            Name = project.Name,
            Description = project.Description,
            RepoAddress = project.RepoAddress
        };

        using (var client = new HttpClient()) {
            var response = await client.PostAsJsonAsync(new Uri($"http://localhost:8080/api/v1/projects"), dto);

            response.EnsureSuccessStatusCode();

            var createdProject = await response.Content.ReadFromJsonAsync<Project>();

            return createdProject;
        }
    }

    private async Task<Project> UpdateProject(Project project) {
        var dto = new UpdateProjectDto {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            RepoAddress = project.RepoAddress
        };

        using (var client = new HttpClient()) {

            var response = await client.PutAsJsonAsync(new Uri($"http://localhost:8080/api/v1/projects/{project.Id}"), dto);

            response.EnsureSuccessStatusCode();

            var updatedProject = await response.Content.ReadFromJsonAsync<Project>();

            return updatedProject;
        }
    }

    public async Task<Note> SaveNote(Note note, Guid projectId) {
        if (note.Id == Guid.Empty) {
            return await AddNote(note, projectId);
        }

        return await UpdateNote(note);
    }

    private async Task<Note> AddNote(Note note, Guid projectId) {
        var dto = new SaveNewNoteDto {
            ProjectId = projectId,
            Content = note.Content
        };

        using (var client = new HttpClient()) {
            var response = await client.PostAsJsonAsync(new Uri($"http://localhost:8080/api/v1/notes"), dto);

            response.EnsureSuccessStatusCode();

            var createdNote = await response.Content.ReadFromJsonAsync<Note>();

            return createdNote;
        }
    }

    private async Task<Note> UpdateNote(Note note) {
        var dto = new UpdateNoteDto {
            Id = note.Id,
            Content = note.Content
        };

        using (var client = new HttpClient()) {

            var response = await client.PutAsJsonAsync(new Uri($"http://localhost:8080/api/v1/notes/{note.Id}"), dto);

            response.EnsureSuccessStatusCode();

            var updatedNote = await response.Content.ReadFromJsonAsync<Note>();

            return updatedNote;

        }
    }

    

    public async Task DeleteNote(Guid noteId) {
        using (var client = new HttpClient()) {
            var response = await client.DeleteAsync(new Uri($"http://localhost:8080/api/v1/notes/{noteId}"));
            response.EnsureSuccessStatusCode();
        }
    }
}
