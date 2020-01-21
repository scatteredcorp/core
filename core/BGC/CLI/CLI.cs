
using System;
using System.Diagnostics.Contracts;
using BGC.CLI;
using CommandLine;
using CommandLine.Text;

namespace BGC.CLI {
    public static class CLI {
        private static ParserResult<object> cmd;
        
        public static void Run(string[] args) {
            cmd = Parser.Default.ParseArguments<
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

        public static void Exit(int code) {
            Console.WriteLine("Exit status code: " + code);
            System.Environment.Exit(code);
        }
    }
}