using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MindProject.Api.Dtos;
using MindProject.Api.Services;

namespace MindProject.Api.EndPoints;

public static class Project {
    public static void RegisterProjectEndPoints(this IEndpointRouteBuilder routes) {
        var projectsRoute = routes.MapGroup("/api/v1/projects");

        projectsRoute.MapGet("/", (IProjectsService service) => service.GetAllProjectsAsync());

        projectsRoute.MapGet("/{id}", (IProjectsService service, Guid id) => service.GetProjectByIdAsync(id));

        projectsRoute.MapPost("/", async (IProjectsService service, [FromBody] CreateNewProjectRequest project) => {
            var createdProject = await service.AddProjectAsync(project);

            return Results.Created($"/api/v1/projects/{createdProject.Id}", createdProject);
        })
        .Produces<ProjectCreatedResponse>(StatusCodes.Status201Created);

        projectsRoute.MapPut("/{id}", async (IProjectsService service, Guid id, [FromBody] UpdateProjectRequest project) => {
            if (id != project.Id) {
                return Results.BadRequest("Project ID mismatch");
            }

            var updatedProject = await service.UpdateProjectAsync(project);

            return Results.Ok(updatedProject);
        })
        .Produces<ProjectUpdatedResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}