using Microsoft.AspNetCore.Mvc;
using MindProject.Api.Dtos;
using MindProject.Api.Services;

namespace MindProject.Api.EndPoints;

public static class Note {
    public static void RegisterNoteEndPoints(this IEndpointRouteBuilder routes) {
        var notesRoute = routes.MapGroup("/api/v1/notes");

        notesRoute.MapPost("/", async (IProjectsService service, CreateNewNoteRequest note) => {
            var createdNote = await service.AddNoteAsync(note);

            return Results.Created(string.Empty, createdNote);
        })
        .Produces<NoteCreatedResponse>(StatusCodes.Status201Created);


        notesRoute.MapPut("/{noteId}", async (IProjectsService service, Guid noteId, UpdateNoteRequest note) => {
            if (noteId != note.Id) {
                return Results.BadRequest("Note ID mismatch");
            }

            var updatedNote = await service.UpdateNoteAsync(note);

            return Results.Ok(updatedNote);
        })
        .Produces<NoteUpdatedResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        notesRoute.MapDelete("/{noteId}", (IProjectsService service, Guid noteId) => service.DeleteNoteAsync(noteId));

    }
}