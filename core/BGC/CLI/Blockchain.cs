using System;
using BGC.Base58;
using CommandLine;

namespace BGC.CLI {
	public static class BlockchainCmd {
		[Verb("init", HelpText = "Initialize blockchain")]
		public class InitOptions {
			[Option(Group = "init", HelpText="Genesis block reward address")]
			public string Address { get; set; } // Base58 address
		}
		
        public static void InitBlockchain(InitOptions opts) {
            Blockchain.Blockchain blockchain = new Blockchain.Blockchain();
            if (!Utils.ValidateAddress(opts.Address)) {
	            Console.WriteLine("Address provided is invalid");
	            CLI.Exit(1);
            }
            byte[] address = Base58Encode.Decode(opts.Address);
            blockchain.Init(address);
            Console.WriteLine("Blockchain successfully initialized!");
        }
	}
	
}
