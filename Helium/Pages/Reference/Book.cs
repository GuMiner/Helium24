namespace Helium.Pages.Reference
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string Protagonist { get; set; } // Young, old, etc.
        public string Analysis { get; set; }
        public int Tone { get; set; } // 1-5 dark to light
        public int SubjectMatter { get; set; } // 1-5 dark to light
        public int Scope { get; set; } // 1 (local) - 5 (galactic)
        public int Rating { get; set; } // 1-5 

        public Book(string title, string author, DateTime published, string protagonist, string analysis, int tone, int subjectMatter, int scope, int rating)
        {
            Title = title;
            Author = author;
            Published = published;
            Protagonist = protagonist;
            Analysis = analysis;


            Tone = tone;
            SubjectMatter = subjectMatter;
            Scope = scope;
            Rating = rating;

            ValidateRank(tone);
            ValidateRank(subjectMatter);
            ValidateRank(scope);
            ValidateRank(rating);
        }

        private void ValidateRank(int rank)
        {
            if (rank < 1 || rank > 5)
            {
                throw new ArgumentOutOfRangeException("Invalid book ranking (should be 1-5)!");
            }
        }
    }
}
