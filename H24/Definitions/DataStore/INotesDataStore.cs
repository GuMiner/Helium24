
namespace H24.Definitions.DataStore
{
    public interface INotesDataStore
    {
        Note GetNote(string id);
        void SaveNote(Note note);
        void UpdateNote(Note note);
    }
}
