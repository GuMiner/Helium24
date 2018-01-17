using Helium24.Definitions;

namespace Helium24.Interfaces
{
    public interface INotesDataStore
    {
        Note GetNote(string id);
        void SaveNote(Note note);
        void UpdateNote(Note note);
    }
}
