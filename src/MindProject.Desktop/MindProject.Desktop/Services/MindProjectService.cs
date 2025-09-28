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
    Task<Project> GetProject(int projectId);
    Task SaveNote(Note note, int projectId);
    Task DeleteNote(int noteId);
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

    public async Task<Project> GetProject(int id) {
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

    public async Task SaveNote(Note note, int projectId) {

        if (note.Id > 0) {
            UpdateNote(note);
        }

        await AddNote(note, projectId);
    }

    private async Task UpdateNote(Note note) {
        var dto = new UpdateNoteDto {
            NoteId = note.Id,
            Content = note.Content
        };

        using (var client = new HttpClient()) {
            var response = await client.PutAsJsonAsync(new Uri($"http://localhost:8080/api/v1/notes/{note.Id}"), dto);
            response.EnsureSuccessStatusCode();
        }
    }

    private async Task AddNote(Note note, int projectId) {
        var dto = new SaveNewNoteDto {
            ProjectId = projectId,
            Content = note.Content
        };

        using (var client = new HttpClient()) {
            var response = await client.PostAsJsonAsync(new Uri($"http://localhost:8080/api/v1/notes"), dto);

            response.EnsureSuccessStatusCode();
        }
    }

    public async Task DeleteNote(int noteId) {
        using (var client = new HttpClient()) {
            var response = await client.DeleteAsync(new Uri($"http://localhost:8080/api/v1/notes/{noteId}"));
            response.EnsureSuccessStatusCode();
        }
    }
}
