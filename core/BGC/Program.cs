using System;
using System.Threading;
using BGC.Blockchain;
using BGC.Contracts;
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