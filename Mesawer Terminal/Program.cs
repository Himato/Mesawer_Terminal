using System;
using Mesawer;

namespace MesawerTerminal
{
    internal class Program
    {
        private static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += ExitHandler;
            Terminal.Run();
        }

        private static void ExitHandler(object sender, EventArgs args)
        {
            Terminal.Save();
        }
    }
}
