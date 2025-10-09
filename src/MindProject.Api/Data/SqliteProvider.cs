using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MindProject.Api.Data.Mappers;
using MindProject.Api.Models;

namespace MindProject.Api.Data;

public interface IDatabaseProvider {
    Task<List<Project>> GetProjectsAsync();
    Task<Project> GetProjectAsync(Guid id);
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task<Note> GetNoteAsync(Guid noteId);
    Task AddNoteAsync(Guid projectId, Note note);
    Task UpdateNoteAsync(Note note, Guid projectId);
    Task DeleteNoteAsync(Guid noteId);
}

public class SqliteProvider : IDatabaseProvider {
    private IConfiguration _configuration;

    public SqliteProvider(IConfiguration configuration) {
        _configuration = configuration;
        InitializeDb();
    }

    private void InitializeDb() {
        var query = """
        CREATE TABLE IF NOT EXISTS Projects (
            Id TEXT PRIMARY KEY,
            Name TEXT NOT NULL,
            Description TEXT,
            RepoAddress TEXT,
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            EndedAt DATETIME
        );

        CREATE TABLE IF NOT EXISTS Notes (
            Id TEXT PRIMARY KEY,
            ProjectId TEXT NOT NULL,
            Content TEXT,
            CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        );
        """;

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            db.Open();
            var command = new SqliteCommand(query, db);
            command.ExecuteReader();
        }

    }

    public async Task<List<Project>> GetProjectsAsync() {
        var projects = new List<Project>();

        var query = """
        SELECT P.Id, P.Name, P.Description, P.RepoAddress, P.CreatedAt, P.UpdatedAt, P.EndedAt 
        FROM Projects P 
        ORDER BY UpdatedAt asc;
        """;

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            db.Open();

            var reader = await command.ExecuteReaderAsync();

            projects = reader.ToProjects().ToList();
        }

        return projects;
    }

    public async Task<Project> GetProjectAsync(Guid id) {
        var query = """
        SELECT P.Id, P.Name, P.Description, P.RepoAddress, P.CreatedAt, P.UpdatedAt, P.EndedAt, 
            N.Id, N.Content, N.CreatedAt, N.UpdatedAt
        FROM Projects P
        LEFT JOIN Notes N ON P.Id LIKE N.ProjectId
        WHERE P.Id LIKE @Id;
        """;

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", id.ToString());

            db.Open();
            var reader = await command.ExecuteReaderAsync();
            var projects = reader.ToProjects().ToList();

            return projects.FirstOrDefault();
        }
    }

    public async Task AddProjectAsync(Project project) {
        var query = "INSERT INTO Projects (Id, Name, Description, RepoAddress, CreatedAt, UpdatedAt) " +
            "VALUES (@Id, @Name, @Description, @RepoAddress, @CreatedAt, @UpdatedAt)";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", project.Id.ToString());
            command.Parameters.AddWithValue("@Name", project.Name);
            command.Parameters.AddWithValue("@Description", project.Description);
            command.Parameters.AddWithValue("@RepoAddress", project.RepoAddress);
            command.Parameters.AddWithValue("@CreatedAt", project.CreatedAt.ToString());
            command.Parameters.AddWithValue("@UpdatedAt", project.UpdatedAt.ToString());
            // command.Parameters.AddWithValue("@EndedAt", project.EndedAt?.ToString());

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateProjectAsync(Project project) {
        var query = "UPDATE Projects SET Name = @Name, Description = @Description, " +
            "RepoAddress = @RepoAddress, UpdatedAt = @UpdatedAt, EndedAt = @EndedAt " +
            "WHERE Id LIKE @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Name", project.Name);
            command.Parameters.AddWithValue("@Description", project.Description);
            command.Parameters.AddWithValue("@RepoAddress", project.RepoAddress);
            command.Parameters.AddWithValue("@UpdatedAt", project.UpdatedAt.ToString());
            command.Parameters.AddWithValue("@EndedAt", project.EndedAt == null ? DBNull.Value : project.EndedAt?.ToString());
            command.Parameters.AddWithValue("@Id", project.Id.ToString());

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<Note> GetNoteAsync(Guid noteId) {
        var query = "SELECT Id, Content, CreatedAt, UpdatedAt FROM Notes WHERE Id LIKE @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", noteId.ToString());

            db.Open();
            var reader = await command.ExecuteReaderAsync();
            var notes = reader.ToNotes().ToList();

            return notes.FirstOrDefault();
        }
    }

    public async Task AddNoteAsync(Guid projectId, Note note) {
        var query = "INSERT INTO Notes (Id, ProjectId, Content, CreatedAt, UpdatedAt) " +
            "VALUES (@Id, @ProjectId, @Content, @CreatedAt, @UpdatedAt)";

        var queryUpdateProject = "UPDATE Projects SET UpdatedAt = @UpdatedAt WHERE Id LIKE @Id";


        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            db.Open();

            using (var transaction = db.BeginTransaction(deferred: true)) {
                using (var insertCmd = new SqliteCommand(query, db)) {
                    insertCmd.Parameters.AddWithValue("@Id", note.Id.ToString());
                    insertCmd.Parameters.AddWithValue("@ProjectId", projectId.ToString());
                    insertCmd.Parameters.AddWithValue("@Content", note.Content);
                    insertCmd.Parameters.AddWithValue("@CreatedAt", note.CreatedAt);
                    insertCmd.Parameters.AddWithValue("@UpdatedAt", note.UpdatedAt);

                    await insertCmd.ExecuteNonQueryAsync();
                }

                using (var updateCmd = new SqliteCommand(queryUpdateProject, db)) {
                    updateCmd.Parameters.AddWithValue("@UpdatedAt", note.UpdatedAt);
                    updateCmd.Parameters.AddWithValue("@Id", projectId.ToString());

                    await updateCmd.ExecuteNonQueryAsync();
                }
                
                transaction.Commit();
            }
        }
    }

    public async Task UpdateNoteAsync(Note note, Guid projectId) {
        var query = "UPDATE Notes SET Content = @Content, UpdatedAt = @UpdatedAt WHERE Id LIKE @Id";
        var queryUpdateProject = "UPDATE Projects SET UpdatedAt = @UpdatedAt WHERE Id LIKE @Id";


        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            db.Open();

            using (var transaction = db.BeginTransaction(deferred: true)) {
                using (var updateCmd = new SqliteCommand(query, db)) {
                    updateCmd.Parameters.AddWithValue("@Content", note.Content);
                    updateCmd.Parameters.AddWithValue("@UpdatedAt", note.UpdatedAt);
                    updateCmd.Parameters.AddWithValue("@Id", note.Id.ToString());

                    await updateCmd.ExecuteNonQueryAsync();
                }

                using (var updateCmd = new SqliteCommand(queryUpdateProject, db)) {
                    updateCmd.Parameters.AddWithValue("@UpdatedAt", note.UpdatedAt);
                    updateCmd.Parameters.AddWithValue("@Id", projectId.ToString());

                    await updateCmd.ExecuteNonQueryAsync();
                }

                transaction.Commit();
            }
        }
    }
    
    public async Task DeleteNoteAsync(Guid noteId) {
        var query = "DELETE FROM Notes WHERE Id LIKE @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", noteId.ToString());

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }
}