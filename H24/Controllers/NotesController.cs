using H24.Definitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace H24.Modules
{
    /// <summary>
    /// Module for retrieving the status of various server resources.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger logger;

        public NotesController(ILogger<NotesController> logger)
        {
            this.logger = logger;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CurrentNotes()
        {
            string id = this.HttpContext.GetId();
            if (this.HttpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                Note note = Program.NotesStore.GetNote(id);
                if (note == null)
                {
                    note = new Note()
                    {
                        Id = id,
                        Notes = "// Enter notes here",
                        LastEdit = DateTime.MinValue,
                        LastNotes = string.Empty
                    };

                    Program.NotesStore.SaveNote(note);
                }

                return this.Ok(new
                {
                    Notes = note.Notes,
                    LastEditDate = note.LastEdit.ToString("MMMM dd, yyyy")
                });
            }
            else
            {
                Note existingNotes = Program.NotesStore.GetNote(id);
                string sourceNotes = existingNotes != null ? existingNotes.Notes : string.Empty;

                Note note = new Note()
                {
                    Id = id,
                    Notes = new StreamReader(this.Request.Body).ReadToEnd(),
                    LastNotes = sourceNotes,
                    LastEdit = DateTime.UtcNow
                };

                Program.NotesStore.UpdateNote(note);
                return this.Ok("Notes saved!");
            }
        }

        public IActionResult LastNotes()
        {
            string id = this.HttpContext.GetId();
            Note note = Program.NotesStore.GetNote(id);
            return this.Ok(note.LastNotes ?? string.Empty);
        }
    }
}