using System;
using System.Collections.Generic;
using H24.Models;

namespace H24
{
    /// <summary>
    /// Caches the list of documentation we have from our PostgreSQL server.
    /// </summary>
    internal class DocumentationCache
    {
        private Object documentCacheUpdaterLock = new Object();
        private List<PiCachedDocument> documentCache = null;

        private DateTime lastSyncTime;
        private TimeSpan syncDelay;

        public DocumentationCache(int syncDelayInHours = 24)
        {
            syncDelay = TimeSpan.FromHours(syncDelayInHours);
            lastSyncTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets all the documents from the server, updating from the documentation cache if needed.
        /// </summary>
        public IEnumerable<PiCachedDocument> GetAllDocuments()
        {
            lock (documentCacheUpdaterLock)
            {
                if (documentCache == null || lastSyncTime + syncDelay < DateTime.UtcNow)
                {
                    documentCache = Program.DocsStore.GetDocuments();
                    lastSyncTime = DateTime.UtcNow;
                    // TODO: Global.Log($"Documentation cache updated at {lastSyncTime}.");
                }
            }

            return documentCache;
        }
    }
}
