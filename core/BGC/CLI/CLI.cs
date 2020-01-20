using System;
using System.Text;
using BGC.Base58;
using CommandLine;

namespace BGC.CLI {
    [Verb("init", HelpText = "Initialize blockchain")]
    class InitOptions {
        [Option(Group = "init", HelpText="Genesis block reward address")]
        public string Address { get; set; }
    }
    [Verb("createwallet", HelpText = "Create a new wallet")]
    class CreateWalletOptions {
    }
    [Verb("inventory", HelpText = "Display list of owned marbles")]
    class InventoryOptions {
    }
    public static class CLI {
        public static void Run(string[] args) {
            Parser.Default.ParseArguments<InitOptions, CreateWalletOptions, InventoryOptions>(args)
                .WithParsed<InitOptions>(InitBlockchainCmd)
                .WithParsed<CreateWalletOptions>(CreateWalletCmd)
                .WithParsed<InventoryOptions>(GetInventoryCmd);
        }
        
        private static void InitBlockchainCmd(InitOptions opts) {
            Blockchain.Blockchain blockchain = new Blockchain.Blockchain();
            Console.WriteLine("Address: " + opts.Address);
            blockchain.Init(new byte[20]);
        }

        private static void CreateWalletCmd(CreateWalletOptions opts) {
            Console.WriteLine("Create a new wallet.");
            throw new NotImplementedException();
        }

        private static void GetInventoryCmd(InventoryOptions opts) {
            throw new NotImplementedException();
        }

    }
    
    
}