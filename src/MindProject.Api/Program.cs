using Microsoft.AspNetCore.Mvc;
using MindProject.Api.Data;
using MindProject.Api.Dtos;
using MindProject.Api.EndPoints;
using MindProject.Api.Models;
using MindProject.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProjectsService, ProjectsService>();
builder.Services.AddSingleton<IDatabaseProvider, SqliteProvider>();

builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument(config => {
    config.DocumentName = "Mind Project";
    config.Title = "Mind Project API";
    config.Version = "v1";
});

var app = builder.Build();

// if (app.Environment.IsDevelopment()) {
    app.UseOpenApi();
    app.UseSwaggerUi(config => {
        config.DocumentTitle = "Mind Project API";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });

    app.MapOpenApi();
// }

// Register Endpoints
app.RegisterProjectEndPoints();
app.RegisterNoteEndPoints();

app.UseHttpsRedirection();

app.Run();