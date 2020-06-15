using System;
using System.Runtime.CompilerServices;

namespace Mesawer
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public CommandAttribute([CallerMemberName] string name = null)
        {
            Name = name;
        }
    }
}
