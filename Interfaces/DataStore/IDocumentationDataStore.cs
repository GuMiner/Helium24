using Helium24.Models;
using System.Collections.Generic;

namespace Helium24.Interfaces
{
    public interface IDocumentationDataStore
    {
        List<PiCachedDocument> GetDocuments();
    }
}
