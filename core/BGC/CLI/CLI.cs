
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
                    BlockchainCmd.ShowBlocksOptions,
                    BlockchainCmd.MineOptions,

                    ContractsCmd.SignContractOptions,
                    ContractsCmd.DecodeContractOptions,

                    WalletCmd.CreateWalletOptions,
                    WalletCmd.GetWalletOptions,
                    WalletCmd.DeleteWalletOptions,

                    InventoryCmd.InventoryOptions,
                    NetworkCmd.StartOptions,
                    NetworkCmd.SendDataOptions
                >(args)
                // Blockchain related commands
                .WithParsed<BlockchainCmd.InitOptions>(BlockchainCmd.InitBlockchain)
                .WithParsed<BlockchainCmd.ShowBlocksOptions>(BlockchainCmd.ShowBlocks)
                .WithParsed<BlockchainCmd.MineOptions>(BlockchainCmd.Mine)

                // Wallet related commands
                .WithParsed<WalletCmd.CreateWalletOptions>(WalletCmd.CreateWallet)
                .WithParsed<WalletCmd.GetWalletOptions>(WalletCmd.GetWallet)
                .WithParsed<WalletCmd.DeleteWalletOptions>(WalletCmd.DeleteWallet)

                // Inventory related commands
                .WithParsed<InventoryCmd.InventoryOptions>(InventoryCmd.GetInventory)

                // Contracts related commands
                .WithParsed<ContractsCmd.SignContractOptions>(ContractsCmd.SignContract)
                .WithParsed<ContractsCmd.DecodeContractOptions>(ContractsCmd.DecodeContract)

                .WithParsed<NetworkCmd.StartOptions>(NetworkCmd.Start)
                .WithParsed<NetworkCmd.SendDataOptions>(NetworkCmd.SendData);



        }

        public static void Exit(int code) {
            Console.WriteLine("Exit status code: " + code);
            System.Environment.Exit(code);
        }
    }
}