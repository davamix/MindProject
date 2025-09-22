using Microsoft.AspNetCore.Mvc;
using MindProject.Api.Dtos;
using MindProject.Api.Services;

namespace MindProject.Api.EndPoints;

public static class Project {
    public static void RegisterProjectEndPoints(this IEndpointRouteBuilder routes) {
        var projectsRoute = routes.MapGroup("/api/v1/projects");

        projectsRoute.MapGet("/", (IProjectsService service) => service.GetAllProjectsAsync());

        projectsRoute.MapGet("/{id}", (IProjectsService service, int id) => service.GetProjectByIdAsync(id));

        projectsRoute.MapPost("/", (IProjectsService service, [FromBody] CreateNewProjectRequest project) => service.AddProjectAsync(project));

        projectsRoute.MapPut("/{id}", async (IProjectsService service, int id, [FromBody] UpdateProjectRequest project) => {
            if (id != project.Id) {
                return Results.BadRequest("Project ID mismatch");
            }

            await service.UpdateProjectAsync(project);
            return Results.NoContent();
        });
    }
}