using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BGC.Base58;
using BGC.Contracts;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);
            
            Wallet.Wallet wallet = new Wallet.Wallet(Utils.FromHex("0195ED8967F1565E686BE9B43B7171FE59094E9468B9D4F2FE8429CF490B7FB4"));
            Console.WriteLine(wallet.EncodedAddress());
        }
        
    }
}