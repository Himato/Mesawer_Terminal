using System;

namespace Mesawer.Exceptions
{
    internal class InvalidCommandException : ArgumentException
    {
        public InvalidCommandException(string command): base($"'{command}' is not recognized as an internal or external command") { }
    }
}
