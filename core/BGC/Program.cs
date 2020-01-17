using System;
using System.Collections.Generic;
using System.Linq;
using BGC.Contracts;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            Console.WriteLine("BGC - Core");

            var placement = new Contracts.Placement();
            placement.Add(0, 10);
            placement.Add(2, 55);

            var contract = new Contracts.ThrowContract(placement, 50, new byte[32], 5, 50);
            
            Console.WriteLine(string.Join(" ", contract.Serialize()));
            
            ThrowContract c = Contracts.ContractHelper.DeserializeThrowContract(contract.Serialize());
            
            Console.WriteLine(string.Join(" ", c.Serialize()));
            
            
        }
    }
}