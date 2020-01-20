using System;
using System.Collections.Generic;
using System.Linq;
using BGC.Contracts;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            Console.WriteLine("BGC - Core");
            CLI.CLI.Run(args);
        }
    }
}