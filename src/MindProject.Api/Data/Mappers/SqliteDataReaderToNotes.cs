using Microsoft.Data.Sqlite;
using MindProject.Api.Models;

namespace MindProject.Api.Data.Mappers;

public static class SqliteDataReaderToNotes {
    public static IEnumerable<Note> ToNotes(this SqliteDataReader reader) {
        var notes = new List<Note>();

        while (reader.Read()) {
            var note = new Note {
                Id = reader.GetString(0).ToGuid(),
                Content = reader.GetString(1),
                CreatedAt = DateTime.Parse(reader.GetString(2)),
                UpdatedAt = DateTime.Parse(reader.GetString(3))
            };

            notes.Add(note);
        }

        return notes;
    }
    
    private static Guid ToGuid(this string value) => Guid.Parse(value);
}