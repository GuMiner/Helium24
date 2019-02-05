using H24.Models;
using System.Collections.Generic;

namespace H24.Definitions.DataStore
{
    public interface IDocumentationDataStore
    {
        List<PiCachedDocument> GetDocuments();
    }
}
