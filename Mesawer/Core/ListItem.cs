using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mesawer.Core
{
    internal abstract class ListItem
    {
        protected List<Item> Items;

        protected void Load(string path)
        {
            Items = new List<Item>();
            try
            {
                var pathsList = File.ReadAllLines(path).ToList();
                foreach (var entry in pathsList)
                {
                    try
                    {
                        if (!Regex.Match(entry, @"^(\w*);(.*)$").Success) continue;

                        var temp = entry.Split(Shared.DataSeparator);
                        Items.Add(new Item(temp[0], temp[1]));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch
            {
                File.WriteAllText(path, "");
                Items = new List<Item>();
            }
        }

        public Item this[string name]
        {
            get
            {
                return Items.Find(c => c.Name.Equals(name));
            }
            private set
            {
                var i = Items.FindIndex(c => c.Name.Equals(name));
                Items[i] = value;
            }
        }

        public void AddItem(Item item)
        {
            bool isFolders;
            switch (this)
            {
                case Folders _:
                    isFolders = true;
                    break;
                case Files _:
                    isFolders = false;
                    break;
                default:
                    throw new ArgumentException("This item can be added either to Folders or Files types");
            }

            var matchedName = Items.Find(entry => entry.Equals(item.Name));
            var matchedPath = Items.Find(entry => entry.Equals(item.Value));

            var addedSuccessfullyMessage = $"'{item.Name}' has been added successfully";
            var replacedSuccessfullyMessage = $"'{item.Name}' 's path has been replaced successfully";

            const string directoryWrongMessage = "This path is not recognized as a folder or an existing path";
            const string fileWrongMessage =
                "This path is not recognized as an operable program, batch file or an existing path";
            const string sameEntryMessage = "This name already exists with the same path.";

            var nameExistsMessage = $"This name already exists with the path: '{matchedName?.Value}'";
            var pathExistsMessage = $"This path already exists under the name '{matchedPath?.Name}'";

            if (!(matchedName != null || matchedPath != null))
            {
                if ((isFolders && Directory.Exists(item.Value)) || (!isFolders && File.Exists(item.Value)))
                {
                    Items.Add(item);
                    Console.WriteLine(addedSuccessfullyMessage);
                }
                else
                {
                    Console.WriteLine((isFolders) ? directoryWrongMessage : fileWrongMessage);
                }
            }
            else if (matchedName != null && matchedPath != null && matchedName.Name.Equals(matchedPath.Name))
            {
                Console.WriteLine(sameEntryMessage);
            }
            else if (matchedPath != null)
            {
                Console.WriteLine(pathExistsMessage);
            }
            else
            {
                Console.WriteLine(nameExistsMessage);

                Console.Write("Do you want to replace it (Y/N)? ");
                var answer = Console.ReadLine();
                if (answer == null || !answer.Equals("y", StringComparison.InvariantCultureIgnoreCase)) return;

                if ((isFolders && Directory.Exists(item.Value)) || (!isFolders && File.Exists(item.Value)))
                {
                    this[item.Name] = item;
                    Console.WriteLine(replacedSuccessfullyMessage);
                }
                else
                {
                    Console.WriteLine((isFolders) ? directoryWrongMessage : fileWrongMessage);
                }
            }
        }

        public void RemoveItem(string name)
        {
            var i = Items.FindIndex(c => c.Name.Equals(name));
            if (i != -1)
            {
                Items.RemoveAt(i);
                Console.WriteLine("'{0}' has been removed successfully", name);
            }
            else Console.WriteLine("There's no such an item.");
        }

        public void ChangeName(string oldName, string newName)
        {
            var i = Items.FindIndex(c => c.Name.Equals(oldName));
            if (i != -1)
            {
                if (Items.All(folder => folder.Equals(newName))) return;

                Items[i].Name = newName;
                Console.WriteLine("'{0}' has moved to '{1}'", oldName, newName);
            }
            else Console.WriteLine("There's no such an item with the name '{0}'", oldName);
        }

        public void PrintItems()
        {
            if (Items.Count != 0)
            {
                foreach (var item in Items)
                {
                    item.Print();
                }
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        protected void Save(string path)
        {
            try
            {
                var output = Items.Aggregate("", (current, item)
                    => current + item.Name + Shared.DataSeparator + item.Value + Environment.NewLine);

                File.WriteAllText(path, output);
            }
            catch
            {
                Console.WriteLine("Unable to save");
            }
        }
    }
}