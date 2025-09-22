using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MindProject.Api.Data.Mappers;
using MindProject.Api.Models;

namespace MindProject.Api.Data;

public interface IDatabaseProvider {
    Task<List<Project>> GetProjectsAsync();
    Task<Project> GetProjectAsync(int id);
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task<Note> GetNoteAsync(int noteId);
    Task AddNoteAsync(int projectId, Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(int noteId);
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
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Description TEXT,
            RepoAddress TEXT,
            Commits TEXT,
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            EndedAt DATETIME
        );

        CREATE TABLE IF NOT EXISTS Notes (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ProjectId INTEGER NOT NULL,
            Content TEXT,
            CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
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

    public async Task<Project> GetProjectAsync(int id) {
        var query = """
        SELECT P.Id, P.Name, P.Description, P.RepoAddress, P.CreatedAt, P.UpdatedAt, P.EndedAt, 
            N.Id, N.Content, N.CreatedAt, N.UpdatedAt
        FROM Projects P
        LEFT JOIN Notes N ON P.Id = N.ProjectId
        WHERE P.Id = @Id;
        """;

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", id);

            db.Open();
            var reader = await command.ExecuteReaderAsync();
            var projects = reader.ToProjects().ToList();

            return projects.FirstOrDefault();
        }
    }

    public async Task AddProjectAsync(Project project) {
        var query = "INSERT INTO Projects (Name, Description, RepoAddress, CreatedAt, UpdatedAt) " +
            "VALUES (@Name, @Description, @RepoAddress, @CreatedAt, @UpdatedAt)";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Name", project.Name);
            command.Parameters.AddWithValue("@Description", project.Description);
            command.Parameters.AddWithValue("@RepoAddress", project.RepoAddress);
            // command.Parameters.AddWithValue("@Commits", project.Commits);
            command.Parameters.AddWithValue("@CreatedAt", project.CreatedAt.ToString());
            command.Parameters.AddWithValue("@UpdatedAt", project.UpdatedAt.ToString());
            // command.Parameters.AddWithValue("@EndedAt", project.EndedAt?.ToString());

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateProjectAsync(Project project) {
        var query = "UPDATE Projects SET Name = @Name, Description = @Description, " +
            "RepoAddress = @RepoAddress, Commits = @Commits, CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt, EndedAt = @EndedAt " +
            "WHERE Id = @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Name", project.Name);
            command.Parameters.AddWithValue("@Description", project.Description);
            command.Parameters.AddWithValue("@RepoAddress", project.RepoAddress);
            command.Parameters.AddWithValue("@Commits", project.Commits);
            command.Parameters.AddWithValue("@CreatedAt", project.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", project.UpdatedAt);
            command.Parameters.AddWithValue("@EndedAt", project.EndedAt);
            command.Parameters.AddWithValue("@Id", project.Id);

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<Note> GetNoteAsync(int noteId) {
        var query = "SELECT Id, Content, CreatedAt, UpdatedAt FROM Notes WHERE Id = @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", noteId);

            db.Open();
            var reader = await command.ExecuteReaderAsync();
            var notes = reader.ToNotes().ToList();

            return notes.FirstOrDefault();
        }
    }
    
    public async Task AddNoteAsync(int projectId, Note note) {
        var query = "INSERT INTO Notes (ProjectId, Content, CreatedAt, UpdatedAt) " +
            "VALUES (@ProjectId, @Content, @CreatedAt, @UpdatedAt)";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@ProjectId", projectId);
            command.Parameters.AddWithValue("@Content", note.Content);
            command.Parameters.AddWithValue("@CreatedAt", note.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", note.UpdatedAt);

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateNoteAsync(Note note) {
        var query = "UPDATE Notes SET Content = @Content, UpdatedAt = @UpdatedAt WHERE Id = @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Content", note.Content);
            command.Parameters.AddWithValue("@UpdatedAt", note.UpdatedAt);
            command.Parameters.AddWithValue("@Id", note.Id);

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }
    
    public async Task DeleteNoteAsync(int noteId) {
        var query = "DELETE FROM Notes WHERE Id = @Id";

        using (var db = new SqliteConnection(_configuration["ConnectionStrings:SqliteConnectionString"])) {
            var command = new SqliteCommand(query, db);
            command.Parameters.AddWithValue("@Id", noteId);

            db.Open();
            await command.ExecuteNonQueryAsync();
        }
    }
}