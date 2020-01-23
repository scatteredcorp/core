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
            
            Wallet.Wallet wallet = new Wallet.Wallet(Utils.FromHex("CB186C327B41EF5F775E531B4FF1C2E555F05C41FBB9CD68F4CC6CBBFD4FB8F1"));
            Console.WriteLine(wallet.EncodedAddress());
        }
        
    }
}