using Nancy;
using System;

namespace Helium24
{
    /// <summary>
    /// Modifies the rood path to get it from the current working directory.
    /// </summary>
    internal class Helium24RootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return Environment.CurrentDirectory;
        }
    }
}