using Helium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Helium.Controllers
{
    /// <summary>
    /// Module for retrieving crossword results
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class CrosswordSearchController : ControllerBase
    {
        private readonly PuzzleDbContext db;

        public CrosswordSearchController(PuzzleDbContext db)
        {
            this.db = db;
        }

        [AcceptVerbs("GET")]
        public IActionResult FindMatchingWords([FromQuery] string? search)
        {
            search = search?.ToUpperInvariant() ?? string.Empty;
            return this.Ok(new
            {
                count = GetResultCount(search),
                clueResults = GetResults(search, GetClueResultQuery),
                answerResults = GetResults(search, GetAnswerResultQuery),
            });
        }

        private int GetResultCount(string search)
        {
            return GetClueResultQuery(search).Count() +
                    GetAnswerResultQuery(search).Count();
        }

        private List<string> GetResults(string search, Func<string, IQueryable<Crossword>> queryFunction)
        {
            return queryFunction(search)
                .OrderBy(crossword => crossword.Clue)
                .Select(crossword => $"{crossword.Clue} -> {crossword.Answer}")
                .Take(200)
                .ToList();
        }

        private IQueryable<Crossword> GetClueResultQuery(string search)
            => db.Crosswords.FromSqlInterpolated($"SELECT * FROM crosswords WHERE clue LIKE {search}");

        private IQueryable<Crossword> GetAnswerResultQuery(string search)
           => db.Crosswords.FromSqlInterpolated($"SELECT * FROM crosswords WHERE answer LIKE {search}");
    }
}