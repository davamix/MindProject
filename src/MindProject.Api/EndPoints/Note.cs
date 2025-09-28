using Microsoft.AspNetCore.Mvc;
using MindProject.Api.Dtos;
using MindProject.Api.Services;

namespace MindProject.Api.EndPoints;

public static class Note {
    public static void RegisterNoteEndPoints(this IEndpointRouteBuilder routes) {
        var notesRoute = routes.MapGroup("/api/v1/notes");

        notesRoute.MapPost("/", (IProjectsService service, [FromBody] CreateNewNoteRequest note) => service.AddNoteAsync(note));
        notesRoute.MapPut("/{noteId}", (IProjectsService service, int noteId, [FromBody] UpdateNoteRequest note) => service.UpdateNoteAsync(noteId, note));
        notesRoute.MapDelete("/{noteId}", (IProjectsService service, int noteId) => service.DeleteNoteAsync(noteId));

    }
}