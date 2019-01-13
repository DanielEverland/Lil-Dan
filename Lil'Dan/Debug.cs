using System;
using System.Collections.Generic;
using System.Text;

namespace Lil_Dan
{
    public static class Debug
    {
        public static void Log(object message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }
    }
}
