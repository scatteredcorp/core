using System;
using System.Net;
using BGC.Contracts;
using BGC.Wallet;

namespace BGC {
    internal static class BGC {
        private static void Main(string[] args) {
            CLI.CLI.Run(args);
            
            // Create dummy contract
            Placement fee = new Placement();
            fee.Add(0, 12);
            fee.Add(1, 15);
            
            Placement playerOnePlacement = new Placement();
            playerOnePlacement.Add(2, 64);
            playerOnePlacement.Add(4, 32);

            Placement playerTwoPlacement = new Placement();
            playerTwoPlacement.Add(5, 122);
            playerTwoPlacement.Add(8, 2);
            
            // Dummy wallets
            Wallet.Wallet wallet1 = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            Wallet.Wallet wallet2 = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            
            // Create contract
            StartContract startContract = new StartContract(fee, playerOnePlacement, playerTwoPlacement,
                wallet1.Address(), wallet2.Address());

            startContract.PartialSign(wallet1.PrivateKey, 4);
            startContract.Sign(wallet2.PrivateKey, 8);
            
            Console.WriteLine(Utils.ToHex(startContract.Serialize(ContractHelper.SerializationType.Complete)));

            var listener = new Network.Listener(12000, 32);
            
        }
    }
}