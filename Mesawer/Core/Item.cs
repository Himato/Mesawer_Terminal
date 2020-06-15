using System;
using System.Diagnostics;

namespace Mesawer.Core
{
    internal class Item
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public Item(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public bool Equals(string str)
        {
            if (str == null) return false;

            return str.Equals(Name) || str.Equals(Value);
        }

        public void Start()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Value,
                    UseShellExecute = true,
                    Verb = "open"
                });
                Console.WriteLine("This process has been started ...");
            }
            catch
            {
                Console.WriteLine("The corresponding path to '{0}' is invalid", Name);
            }
        }

        public void Print()
        {
            Console.WriteLine("\t'{0}' -> {1}", Name, Value);
        }
    }
}