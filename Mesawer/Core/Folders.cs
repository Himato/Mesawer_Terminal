
namespace Mesawer.Core
{
    internal class Folders : ListItem
    {
        private const string Path = "./folders.txt";

        public const string Identifier = "/f";

        public const string StringIdentifier = "folders";

        public Folders()
        {
            Load(Path);
        }

        ~Folders()
        {
            Save();
        }

        public void Save()
        {
            Save(Path);
        }
    }
}