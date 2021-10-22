using System;
using System.Text.RegularExpressions;
using Mesawer.Exceptions;

namespace Mesawer
{
    public class Terminal
    {

        public static void Run()
        {
            /*
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
___  ___                                         _____                       _                _ 
|  \/  |                                        |_   _|                     (_)              | |
| .  . |  ___  ___   __ _ __      __ ___  _ __    | |  ___  _ __  _ __ ___   _  _ __    __ _ | |
| |\/| | / _ \/ __| / _` |\ \ /\ / // _ \| '__|   | | / _ \| '__|| '_ ` _ \ | || '_ \  / _` || |
| |  | ||  __/\__ \| (_| | \ V  V /|  __/| |      | ||  __/| |   | | | | | || || | | || (_| || |
\_|  |_/ \___||___/ \__,_|  \_/\_/  \___||_|      \_/ \___||_|   |_| |_| |_||_||_| |_| \__,_||_|

Version: {0}                    Created By: Ibrahim Mesawer                     In: 26 Feb. 2019", Shared.Version);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

    */

            Console.SetWindowSize(56, 16);

            do
            {
                Console.Write("-> ");
                var input = Console.ReadLine()?.Trim() ?? throw new ArgumentException("Failed");
                Commands.Inputs.Add(input);
                var match = Regex.Match(input, @"^(\w*)(.*)$", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    var values = match.Groups;

                    try
                    {
                        Commands.Call(values[1].ToString(), values[2].ToString().Trim());
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("'{0}' is not recognized as an internal or external command", input.Split(' ')[0]);
                }
            } while (true);
        }

        public static void Save()
        {
            Commands.Folders.Save();
            Commands.Files.Save();
        }
    }
}
