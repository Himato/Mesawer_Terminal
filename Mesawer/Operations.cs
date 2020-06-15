using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mesawer
{
    internal static class Operations
    {
        private const string FoldersPath = "./folders.txt";
        private const string ProgramsPath = "./programs.txt";

        private const string InvalidChars = "! @ # $ % ^ & * ( ) - + = ~ | \\ \" ' { } [ ] > < . ? ;";

        public static List<string> GetFolders()
        {
            try
            {
                return File.ReadAllLines(FoldersPath).ToList();
            }
            catch
            {
                File.WriteAllText(FoldersPath, "");
                return new List<string>();
            }
        }

        public static List<string> GetPrograms()
        {
            try
            {
                return File.ReadAllLines(ProgramsPath).ToList();
            }
            catch
            {
                File.WriteAllText(ProgramsPath, "");
                return new List<string>();
            }
        }

        public static void Add(List<string> list, string data, bool isPrograms)
        {
            try
            {
                {
                    var found = list.FirstOrDefault(c =>
                        c.Split(Shared.DataSeparator)[0].Equals(data.Split(Shared.DataSeparator)[0]))?.Split(Shared.DataSeparator);

                    if (found != null)
                    {
                        Console.WriteLine("The name already exists with path: '{0}'", found[1]);
                        return;
                    }
                }

                {
                    var found = list.FirstOrDefault(c =>
                        c.Split(Shared.DataSeparator).Count() == 2 && c.Split(Shared.DataSeparator)[1].Equals(data.Split(Shared.DataSeparator)[1]))?.Split(Shared.DataSeparator);

                    if (found != null)
                    {
                        Console.WriteLine("The Path already exists with name: '{0}'", found[0]);
                        return;
                    }
                }

                var path = isPrograms ? ProgramsPath : FoldersPath;
                var substrings = InvalidChars.Split(' ');
                if (data.Split(Shared.DataSeparator)[0].Contains(substrings))
                {
                    Console.WriteLine("The name can't contain characters other than _");
                    return;
                }

                if (!File.ReadAllText(path).Equals(""))
                {
                    using (var sw = File.AppendText(path))
                    {
                        sw.WriteLine();
                        sw.Write(data);
                    }
                }
                else
                {
                    using (var sw = File.AppendText(path))
                    {
                        sw.Write(data);
                    }
                }

                list.Add(data);
                Console.WriteLine("The item has been added successfully");
            }
            catch
            {
                Console.WriteLine("Something went wrong :(");
            }
        }

        public static void Remove(List<string> list, string data, bool isPrograms)
        {
            var item = list.FirstOrDefault(c => c.Split(Shared.DataSeparator)[0].Equals(data));

            if (item == null)
            {
                Console.WriteLine("Item not Found");
                return;
            }

            try
            {
                var path = isPrograms ? ProgramsPath : FoldersPath;
                list.Remove(data);

                var paths = File.ReadAllText(path);
                var newPaths = paths.Replace("\n" + data, "");
                File.WriteAllText(path, newPaths);
                Console.WriteLine("The item has been removed form this terminal");
            }
            catch
            {
                Console.WriteLine("Something went wrong!");
            }
        }

        public static void Edit(List<string> list, string oldName, string newName, bool isPrograms)
        {
            var item = list.FirstOrDefault(c => c.Split(Shared.DataSeparator)[0].Equals(oldName));

            if (item == null)
            {
                Console.WriteLine("Item not Found");
                return;
            }

            var path = isPrograms ? ProgramsPath : FoldersPath;
            var substrings = InvalidChars.Split(' ');
            if (newName.Contains(substrings))
            {
                Console.WriteLine("The name can't contain characters other than _");
                return;
            }

            var newData =
                $"{path.Split(Shared.DataSeparator)[0].Replace(oldName, newName)}{Shared.DataSeparator}{path.Split(Shared.DataSeparator)[1]}";

            try
            {
                list[list.IndexOf(item)] = newData;
                var paths = File.ReadAllText(path);
                var newPaths = paths.Replace(item, newData);
                File.WriteAllText(path, newPaths);
                Console.WriteLine("Program has been updated");
            }
            catch
            {
                Console.WriteLine("Something went wrong!");
            }
        }

        //public static void AddFolder(List<string> folders, String data)
        //{
        //    try
        //    {
        //        int i = 0;
        //        bool exists = false;
        //        foreach (string e in folders)
        //        {
        //            if (e.Split(Shared.DataSeparator)[0].Equals(data.Split(Shared.DataSeparator)[0]))
        //            {
        //                Console.WriteLine("The name already exists with path: '{0}'", e.Split(Shared.DataSeparator)[1]);
        //                return;
        //            }
        //            else
        //            {
        //                i++;
        //            }
        //            if (i == folders.Count())
        //            {
        //                exists = false;
        //            }
        //        }
        //        if (!exists)
        //        {
        //            i = 0;
        //            foreach (string e in folders)
        //            {
        //                if (e.Split(Shared.DataSeparator).Count() == 2 && e.Split(Shared.DataSeparator)[1].Equals(data.Split(Shared.DataSeparator)[1]))
        //                {
        //                    Console.WriteLine("The Path already exists with name: '{0}'", e.Split(Shared.DataSeparator)[0]);
        //                    return;
        //                }
        //                else
        //                {
        //                    i++;
        //                }
        //                if (i == folders.Count())
        //                {
        //                    exists = false;
        //                }
        //            }
        //        }
        //        if (!exists)
        //        {
        //            string path = "./folders.txt";
        //            string wrongChars = "! @ # $ % ^ & * ( ) - + = ~ | \\ \" ' { } [ ] > < . ? ;";
        //            string[] substrings = wrongChars.Split(' ');
        //            if (data.Split(Shared.DataSeparator)[0].Contains(substrings))
        //            {
        //                Console.WriteLine("The name can't contain characters other than _");
        //            }
        //            else
        //            {
        //                if (!File.ReadAllText(path).Equals(""))
        //                {
        //                    using (StreamWriter sw = File.AppendText(path))
        //                    {
        //                        sw.WriteLine();
        //                        sw.Write(data);
        //                    }
        //                }
        //                else
        //                {
        //                    using (StreamWriter sw = File.AppendText(path))
        //                    {
        //                        sw.Write(data);
        //                    }
        //                }
        //                folders.Add(data);
        //                Console.WriteLine("the folder has been added successfully");
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Something went wrong :(");
        //    }
        //}

        //public static void RemoveFolder(List<string> folders, string data)
        //{
        //    bool isExist = false;
        //    foreach (string path in folders)
        //    {
        //        if (path.Split(Shared.DataSeparator)[0].Equals(data))
        //        {
        //            isExist = true;
        //            data = path;
        //            break;
        //        }
        //    }
        //    if (isExist)
        //    {
        //        try
        //        {
        //            string path = "./folders.txt";
        //            folders.Remove(data);
        //            string paths = File.ReadAllText(path);
        //            string newPaths = paths.Replace("\n" + data, "");
        //            File.WriteAllText(path, newPaths);
        //            Console.WriteLine("Folder has been removed from this terminal");
        //        }
        //        catch
        //        {
        //            Console.WriteLine("Something went wrong!");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Folder not Found");
        //    }
        //}

        //public static void EditFolder(List<string> folders, string oldName, string newName)
        //{
        //    bool isExist = false;
        //    int index = -1;
        //    string oldData = null;
        //    string newData = null;
        //    foreach (string path in folders)
        //    {
        //        if (path.Split(Shared.DataSeparator)[0].Equals(oldName))
        //        {
        //            string wrongChars = "! @ # $ % ^ & * ( ) - + = ~ | \\ \" ' { } [ ] > < . ? ;";
        //            string[] substrings = wrongChars.Split(' ');
        //            if (newName.Split(Shared.DataSeparator)[0].Contains(substrings))
        //            {
        //                Console.WriteLine("The name can't contain characters other than _");
        //                return;
        //            }
        //            else
        //            {
        //                isExist = true;
        //                index = folders.IndexOf(path);
        //                oldData = path;
        //                newData = String.Format("{0}{1}{2}", path.Split(Shared.DataSeparator)[0].Replace(oldName, newName), Shared.DataSeparator, path.Split(Shared.DataSeparator)[1]);
        //            }
        //            break;
        //        }
        //    }
        //    if (isExist)
        //    {
        //        try
        //        {
        //            string path = "./folders.txt";
        //            folders[index] = newData;
        //            string paths = File.ReadAllText(path);
        //            string newPaths = paths.Replace(oldData, newData);
        //            File.WriteAllText(path, newPaths);
        //            Console.WriteLine("Folder has been updated");
        //        }
        //        catch
        //        {
        //            Console.WriteLine("Something went wrong!");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Folder not Found");
        //    }
        //}
    }
}
