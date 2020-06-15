
namespace Mesawer.Core
{
    internal class Files : ListItem
    {
        private const string Path = "./files.txt";

        public const string Identifier = "/a";

        public const string StringIdentifier = "files";

        public Files()
        {
            Load(Path);
        }

        ~Files()
        {
            Save();
        }

        public void Save()
        {
            Save(Path);
        }
    }
}