using System;

namespace Mesawer.Exceptions
{
    internal class InvalidSyntaxException : ArgumentException
    {
        public InvalidSyntaxException(): base(Shared.InvalidSyntax) { }

        public InvalidSyntaxException(string message)
            : base(message) { }

        public InvalidSyntaxException(string message, Exception inner)
            : base(message, inner) { }
    }
}
