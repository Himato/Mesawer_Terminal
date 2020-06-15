using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Mesawer.Core;
using Mesawer.Exceptions;

namespace Mesawer
{
    internal static class Commands
    {
        internal static Folders Folders { get; } = new Folders();
        internal static Files Files { get; } = new Files();
        internal static List<string> Inputs { get; } = new List<string>();

        public static void Call(string command, string data = null)
        {
            if (command.IsNullOrWhitespace())
            {
                return;
            }

            var method = typeof(Commands).GetMethods()
                .FirstOrDefault(c => GetAttribute(c) != null && GetAttribute(c).Name.Equals(command.ToPascalCase()));

            if (method == null)
            {
                throw new InvalidCommandException(command);
            }

            try
            {
                method.Invoke(null, new object[] {data});
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException ?? new Exception();
            }
        }

        [Command(Description = "Adds a program to your files list")]
        public static void Add(string input)
        {
            var match = Regex.Match(input, @"^([/][fa]) (\w*) ""([a-z || A-Z]:\\.*[\\]?)""$");
            if (match.Success)
            {
                var data = match.Groups;
                if (data[1].ToString().Equals(Folders.Identifier))
                {
                    Folders.AddItem(new Item(data[2].ToString(), data[3].ToString()));
                    Folders.Save();
                }
                else if (data[1].ToString().Equals(Files.Identifier))
                {
                    Files.AddItem(new Item(data[2].ToString(), data[3].ToString()));
                    Files.Save();
                }
            }
            else
            {
                throw new InvalidSyntaxException("Invalid Syntax, you should follow one of these formats: \n" +
                                                 "For adding a folder: /f name \"C:\\example\"\n" +
                                                 "For adding a file: /a name \"C:\\example\\app.exe\"");
            }
        }

        [Command(Description = "Clears the history")]
        public static void Clear(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            Inputs.Clear();
            Console.WriteLine("History has been Cleared Successfully");
        }

        [Command(Description = "Closes the app")]
        public static void Exit(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            throw new Exception();
        }

        [Command(Description = "Shows our e-mail")]
        public static void Feedback(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            Console.WriteLine("We'll be more than happy to hear your feedback");
            Console.WriteLine("Please, Contact us on -> himato.gamal120@outlook.com");
        }

        [Command(Description = "Shows helpful commands to use")]
        public static void Help(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            foreach (var command in typeof(Commands).GetMethods().Select(GetAttribute).Where(c => c != null))
            {
                Console.WriteLine("{0, -12} {1}", command.Name.ToUpper(), command.Description);
            }

            Console.WriteLine("********************************* For More Details **********************************");
            Console.WriteLine("                  Visit Our Website: {0}                  ", Shared.Url);
        }

        [Command(Description = "Shows your commands history")]
        public static void History(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            foreach (var str in Inputs)
            {
                Console.WriteLine(str);
            }
        }

        [Command(Name = "Ls", Description = "Lists all the data you added, or specifies the category")]
        public static void List(string input)
        {
            if (input.IsNullOrWhitespace())
            {
                Console.WriteLine("****************** Folders ******************");

                try
                {
                    Folders.PrintItems();
                }
                catch
                {
                    Console.WriteLine("No Folders Found");
                }

                Console.WriteLine("****************** Files ******************");

                try
                {
                    Files.PrintItems();
                }
                catch
                {
                    Console.WriteLine("No Files Found");
                }

                return;
            }

            var match = Regex.Match(input, @"^(\w*)[ ]{0,1}(\w*)$");
            if (!match.Success) throw new InvalidSyntaxException("Invalid Arguments: the valid arguments should be '[folders | files] [' ' | name]'");

            var values = match.Groups;
            if (values[1].ToString().Equals(Folders.StringIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                if (values[2].ToString().IsNullOrWhitespace())
                {
                    Console.WriteLine("****************** Folders ******************");
                    Folders.PrintItems();
                    return;
                }

                var item = Folders[values[2].ToString()];
                if (item != null)
                {
                    item.Print();
                }
                else
                {
                    Console.WriteLine("Folder Not Found");
                }
            } 
            else if (values[1].ToString().Equals(Files.StringIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                if (values[2].ToString().IsNullOrWhitespace())
                {
                    Console.WriteLine("****************** Files ******************");
                    Files.PrintItems();
                    return;
                }

                var item = Files[values[2].ToString()];
                if (item != null)
                {
                    item.Print();
                }
                else
                {
                    Console.WriteLine("File Not Found");
                }
            }
            else
            {
                throw new InvalidSyntaxException("Invalid Arguments: the valid arguments should be '[folders | files] [' ' | name]'");
            }
        }

        [Command(Name = "Mv", Description = "Changes the folder or the program name")]
        public static void Move(string input)
        {
            var match = Regex.Match(input, @"^(\w*) (\w*) (\w*)$");
            if (!match.Success) throw new InvalidSyntaxException("Invalid Arguments: the valid arguments should be '[folders | files] old_name new_name'");

            var values = match.Groups;
            if (values[1].ToString().Equals(Folders.StringIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                var oldName = values[2].ToString();
                var newName = values[3].ToString();
                Folders.ChangeName(oldName, newName);
                Folders.Save();
            }
            else if (values[1].ToString().Equals(Files.StringIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                var oldName = values[2].ToString();
                var newName = values[3].ToString();
                Files.ChangeName(oldName, newName);
                Files.Save();
            }
            else
            {
                throw new InvalidSyntaxException("Invalid Arguments: the valid arguments should be '[folders | files] old_name new_name'");
            }
        }

        [Command(Description = "Opens a specific folder")]
        public static void Open(string input)
        {
            var match = Regex.Match(input, @"^(\w*)$");
            if (match.Success)
            {
                var item = Folders[match.Groups[1].ToString()];
                if (item != null) item.Start();
                else Console.WriteLine("Folder Not Found");
            }
            else
            {
                throw new InvalidSyntaxException();
            }
        }

        [Command(Name = "Rm", Description = "Removes a folder or a program using its name")]
        public static void Remove(string input)
        {
            var match = Regex.Match(input, @"^(\w*) (\w*)$");
            if (!match.Success) throw new InvalidSyntaxException("Invalid Arguments: the valid arguments should be '[folders | files] name'");

            var values = match.Groups;
            if (values[1].ToString().Equals(Folders.StringIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                var name = values[2].ToString();
                Folders.RemoveItem(name);
                Folders.Save();
            }
            else if (values[1].ToString().Equals(Files.StringIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                var name = values[2].ToString();
                Files.RemoveItem(name);
                Files.Save();
            }
            else
            {
                throw new InvalidSyntaxException("Invalid Arguments: the valid arguments should be '[folders | files] name'");
            }
        }

        [Command(Description = "Runs a specific program")]
        public static void Run(string input)
        {
            var match = Regex.Match(input, @"^(\w*)$");
            if (match.Success)
            {
                var item = Files[match.Groups[1].ToString()];
                if (item != null) item.Start();
                else Console.WriteLine("Program Not Found");
            }
            else
            {
                throw new InvalidSyntaxException();
            }
        }

        [Command(Description = "Opens a timer app")]
        public static void Timer(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            //new Task(() => { MediaTypeNames.Application.Run(new Timer()); }).Start();
            Console.WriteLine("Timer is not supported in the non-windows versions");
        }

        [Command(Description = "Shows the current version of the app")]
        public static void Version(string input)
        {
            if (!input.IsNullOrWhitespace())
            {
                throw new UnknownArgumentsException();
            }

            Console.WriteLine("Version: {0}", Shared.Version);
            UpdateChecker.CheckingSimulation();
        }

        [Command(Name = "Shut", Description = "Shuts down your PC")]
        public static void ShutDown(string input)
        {
            var temp = Regex.Match(input, "^down(.*)&", RegexOptions.IgnoreCase);
            if (temp.Success)
            {
                try
                {
                    var rest = temp.Groups[1].ToString().Split(' ');

                    if (rest.Length == 2)
                    {
                        if (rest[0].Equals("after", StringComparison.InvariantCultureIgnoreCase) &&
                            int.TryParse(rest[1], out var result))
                        {
                            var timer = new Timer(ShutDownCallback, null, 0, result * 1000);
                            Console.WriteLine("Your PC will be turned off after {0} seconds, restart the terminal to cancel.",
                                Convert.ToInt32(result));
                        }
                    }
                    else
                    {
                        ShutDownCallback();
                    }
                }
                catch
                {
                    throw new InvalidSyntaxException("Please, enter a valid amount of time in seconds (e.g. shut down after 3600)");
                }
            }
            else
            {
                throw new InvalidSyntaxException("Please, enter a valid amount of time in seconds (e.g. shut down after 3600)");
            }
        }

        private static void ShutDownCallback(object state = null)
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
        }

        private static CommandAttribute GetAttribute(MethodInfo method)
        {
            return (CommandAttribute) Attribute.GetCustomAttribute(method, typeof(CommandAttribute));
        }
    }
}