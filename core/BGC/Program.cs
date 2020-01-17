using System;
using System.Collections.Generic;
using System.Linq;
using BGC.Contracts;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            Console.WriteLine("BGC - Core");

            var placement = new Contracts.Placement();
            placement.Add(2, 55);
            var contract = new Contracts.StartContract(placement, 50, placement, placement, new byte[20], new byte[20]);
            
            Console.WriteLine(string.Join("\n", contract.Serialize()));
            Console.WriteLine(contract.Serialize().Length);
        }
    }
}