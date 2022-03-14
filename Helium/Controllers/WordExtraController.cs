using Helium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Helium.Controllers
{
    /// <summary>
    /// Module for word search results
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class WordExtraController : ControllerBase
    {
        private readonly PuzzleDbContext db;

        public WordExtraController(PuzzleDbContext db)
        {
            this.db = db;
        }

        [AcceptVerbs("GET")]
        public IActionResult FindHomophones([FromQuery] string search)
        {
            search = search.ToUpperInvariant();
            return this.Ok(new
            {
                count = GetHomophonesResultCount(search),
                results = GetHomophonesResults(search)
            });
        }

        [AcceptVerbs("GET")]
        public IActionResult FindSynonyms([FromQuery] string search)
        {
            search = search.ToUpperInvariant();
            return this.Ok(new
            {
                count = GetThesaurusResultCount(search),
                results = GetThesaurusResults(search),
            });
        }

        private int GetThesaurusResultCount(string search)
        {
            return db.Thesaurus.FromSqlInterpolated($"SELECT * FROM thesaurus WHERE UPPER(word) LIKE {search}")
                .Count();
        }

        private List<string> GetThesaurusResults(string search)
        {
            Dictionary<int, HashSet<string>> synonymIds = new Dictionary<int, HashSet<string>>();

            var entries = db.Thesaurus.FromSqlInterpolated($"SELECT * FROM thesaurus WHERE UPPER(word) LIKE {search} ORDER BY word LIMIT 10")
                .OrderBy(entry => entry.Word)
                .Take(10).ToList();

            foreach (Thesauru entry in entries)
            {
                string synonymIdsEntry = entry.SynonymIds ?? string.Empty;
                List<int> wordSynonymIds = synonymIdsEntry.Split(',')
                    .Select(item => int.TryParse(item, out int num) ? num : -1)
                    .Where(num => num != -1).ToList();
                foreach (int id in wordSynonymIds)
                {
                    if (!synonymIds.ContainsKey(id))
                    {
                        synonymIds.Add(id, new HashSet<string>());
                    }

                    synonymIds[id].Add(entry.Word ?? string.Empty);
                }
            }

            List<string> results = new();
            if (synonymIds.Any())
            {
                results = db.ThesaurusLookups.FromSqlRaw(
                    $"SELECT * FROM thesaurus_lookup WHERE ID IN ({string.Join(',', synonymIds.Keys)})")
                    .Take(500)
                    .Select(entry => $"{string.Join(", ", synonymIds[(int)entry.Id])} -> {entry.SynonymList}")
                    .ToList();
            }

            return results;
        }

        private int GetHomophonesResultCount(string search)
            => GetHomophonesResultQuery(search).Count();

        private List<string> GetHomophonesResults(string search)
        {
            return GetHomophonesResultQuery(search)
                .Select(homophone => homophone.Homophones ?? "")
                .OrderBy(homophone => homophone)
                .Take(50)
                .ToList();
        }

        private IQueryable<Homophone> GetHomophonesResultQuery(string search)
            => db.Homophones.FromSqlInterpolated($"SELECT * FROM homophones WHERE UPPER(homophones) LIKE {search}");
    }
}