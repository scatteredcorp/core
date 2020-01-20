using System;
using System.Text;
using BGC.Base58;
using CommandLine;

namespace BGC.CLI {
    public static class CLI {
        public static void Run(string[] args) {
            Parser.Default.ParseArguments<
                    BlockchainCmd.InitOptions, 
                    WalletCmd.CreateWalletOptions,
                    WalletCmd.GetWalletOptions,
                    InventoryCmd.InventoryOptions
                >(args)
                // Blockchain related commands
                .WithParsed<BlockchainCmd.InitOptions>(BlockchainCmd.InitBlockchain)
                
                // Wallet related commands
                .WithParsed<WalletCmd.CreateWalletOptions>(WalletCmd.CreateWallet)
                .WithParsed<WalletCmd.GetWalletOptions>(WalletCmd.GetWallet)
                // Inventory related commands
                .WithParsed<InventoryCmd.InventoryOptions>(InventoryCmd.GetInventory);
        }
    }
}