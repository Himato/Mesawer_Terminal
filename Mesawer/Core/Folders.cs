
using System;

namespace Mesawer.Core
{
    internal class Folders : ListItem, IDisposable
    {
        private const string Path = "./folders.txt";

        public const string Identifier = "/f";

        public const string StringIdentifier = "folders";

        public Folders()
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