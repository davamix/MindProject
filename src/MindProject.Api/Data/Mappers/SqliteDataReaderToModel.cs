using Microsoft.Data.Sqlite;
using MindProject.Api.Models;

namespace MindProject.Api.Data.Mappers;

public static class SqliteDataReaderToModel {
    public static IEnumerable<Project> ToProjects(this SqliteDataReader reader) {
        var projects = new List<Project>();

        while(reader.Read()) {
            var project = projects.FirstOrDefault(p => p.Id == reader.GetInt32(0));

            if (project == null) {
                project = new Project {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    RepoAddress = reader.GetString(3),
                    // Commits = reader.GetString(4),
                    CreatedAt = DateTime.Parse(reader.GetString(4)),
                    UpdatedAt = DateTime.Parse(reader.GetString(5)),
                    EndedAt = reader.IsDBNull(6) ? null : DateTime.Parse(reader.GetString(6))
                };

                projects.Add(project);
            }

            if (reader.FieldCount <= 7) continue;
            if (reader.IsDBNull(7)) continue;

            var note = new Note {
                Id = reader.GetInt32(7),
                Content = reader.GetString(8),
                CreatedAt = DateTime.Parse(reader.GetString(9)),
                UpdatedAt = DateTime.Parse(reader.GetString(10))
            };

            project.Notes.Add(note);
        }

        return projects;
    }
}