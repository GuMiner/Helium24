using Helium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Helium.Controllers
{
    /// <summary>
    /// Module for word search results
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class WordSearchController : ControllerBase
    {
        private readonly PuzzleDbContext db;

        public WordSearchController(PuzzleDbContext db)
        {
            this.db = db;
        }

        [AcceptVerbs("GET")]
        public IActionResult FindMatchingWords([FromQuery] string? search)
        {
            search = search?.ToUpperInvariant() ?? string.Empty;

            return this.Ok(new
            {
                count = this.GetResultCount(search, GetResultQuery),
                results = this.GetResults(search, GetResultQuery),
            });
        }

        [AcceptVerbs("GET")]
        public IActionResult FindAnagrams([FromQuery] string? search)
        {
            search = search?.ToUpperInvariant() ?? string.Empty;

            return this.Ok(new
            {
                count = this.GetResultCount(search, GetAnagramQuery),
                results = this.GetResults(search, GetAnagramQuery),
            });
        }

        private int GetResultCount(string search, Func<string, IQueryable<Word>> queryFunction)
            => queryFunction(search).Count();

        private List<string> GetResults(string search, Func<string, IQueryable<Word>> queryFunction)
        {
            return queryFunction(search)
                .Select(word => word.WordValue)
                .OrderBy(word => word)
                .Take(200)
                .ToList();
        }

        private IQueryable<Word> GetResultQuery(string search)
            => db.Words.FromSqlInterpolated($"SELECT * FROM words WHERE word LIKE {search}");

        private IQueryable<Word> GetAnagramQuery(string search)
            // Deliberately don't use FromSqlInterpolated as this is raw anagram SQL
            => db.Words.FromSqlRaw($"SELECT * FROM words WHERE {GetAnagramSearchQuery(search)}");

        private string GetAnagramSearchQuery(string search)
        {
            Dictionary<char, int> characters = new();
            foreach (char character in search)
            {
                if (!characters.ContainsKey(character))
                {
                    characters.Add(character, 0);
                }

                characters[character]++;
            }

            string comp = "=";
            if (characters.ContainsKey('_'))
            {
                // Greater than or equals, to support anagrams with unknown characters.
                comp = ">=";
            }

            StringBuilder searchQuery = new();
            searchQuery.Append($"(LENGTH(word) = {search.Length})");
            foreach (KeyValuePair<char, int> keyValuePair in characters)
            {
                if (keyValuePair.Key != '_')
                {
                    searchQuery.Append($" AND (LENGTH(word) - LENGTH(REPLACE(word, '{keyValuePair.Key}', '')) {comp} {keyValuePair.Value})");
                }
            }

            return searchQuery.ToString();
        }
    }
}