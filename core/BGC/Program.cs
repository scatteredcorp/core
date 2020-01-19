using System;
using System.Collections.Generic;
using System.Linq;
using BGC.Contracts;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            Console.WriteLine("BGC - Core");
            
            Blockchain.Blockchain blockchain = new Blockchain.Blockchain();
            blockchain.Init(new byte[20]);
        }
    }
}