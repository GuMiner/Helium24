using Helium24.Definitions;
using Nancy;
using Nancy.Extensions;
using System;

namespace Helium24.Modules
{
    /// <summary>
    /// Handles retrieving and saving temporary notes.
    /// </summary>
    public class NotesModule : NancyModule
    {
        public NotesModule()
            : base("/Notes")
        {
            // Retreives the notes for the current user.
            Get["/CurrentNotes"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    Note note = Global.NotesStore.GetNote(user.UserName);
                    if (note == null)
                    {
                        note = new Note()
                        {
                            Id = user.UserName,
                            Notes = "// Enter notes here",
                            LastEdit = DateTime.MinValue,
                            LastNotes = string.Empty
                        };

                        Global.NotesStore.SaveNote(note);
                    }
                    return this.Response.AsJson(new
                    {
                        Notes = note.Notes,
                        LastEditDate = note.LastEdit.ToString("MMMM dd, yyyy")
                    });
                });
            };

            // Retrieves the last notes the user.
            Get["/LastNotes"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    Note note = Global.NotesStore.GetNote(user.UserName);
                    return note.LastNotes ?? string.Empty;
                });
            };

            // Saves the notes for the current user.
            Post["/CurrentNotes"] = parameters =>
            {
                return this.Authenticate((user) =>
                {
                    Note existingNotes = Global.NotesStore.GetNote(user.UserName);
                    string sourceNotes = existingNotes != null ? existingNotes.Notes : string.Empty;

                    Note note = new Note()
                    {
                        Id = user.UserName,
                        Notes = this.Request.Body.AsString(),
                        LastNotes = sourceNotes,
                        LastEdit = DateTime.UtcNow
                    };

                    Global.NotesStore.UpdateNote(note);
                    return "Notes saved!";
                });
            };
        }
    }
}
