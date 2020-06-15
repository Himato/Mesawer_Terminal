using System;

namespace Mesawer.Exceptions
{
    internal class UnknownArgumentsException : ArgumentException
    {
        public UnknownArgumentsException(): base(Shared.UnknownArguments) { }
    }
}
