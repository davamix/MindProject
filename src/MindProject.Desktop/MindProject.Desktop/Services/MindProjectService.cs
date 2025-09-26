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
}
