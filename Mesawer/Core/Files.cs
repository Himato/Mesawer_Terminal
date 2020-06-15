
using System;

namespace Mesawer.Core
{
    internal class Files : ListItem, IDisposable
    {
        private const string Path = "./files.txt";

        public const string Identifier = "/a";

        public const string StringIdentifier = "files";

        public Files()
        {
            Load(Path);
        }

        public void Save()
        {
            Save(Path);
        }

        public void Dispose()
        {
            Save();
        }
    }
}