using System;
<<<<<<< HEAD
using System.Threading;
using BGC.Blockchain;
=======
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using BGC.Base58;
>>>>>>> 7f30d5952ac3212d0290fa2cabd321eabc02d755
using BGC.Contracts;
using BGC.Network;
using BGC.Wallet;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);

            Wallet.Wallet wallet = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            //Blockchain.Blockchain.Init(wallet.Address());
            Blockchain.Blockchain.Resume();
            Console.WriteLine(Utils.ToHex(Consensus.Mining.Target()));
        }
    }
}