using System;
using CommandLine;

namespace BGC.CLI {
	public static class BlockchainCmd {
		[Verb("init", HelpText = "Initialize blockchain")]
		public class InitOptions {
			[Option(Group = "init", HelpText="Genesis block reward address")]
			public string Address { get; set; }
		}
		
        public static void InitBlockchain(InitOptions opts) {
            Blockchain.Blockchain blockchain = new Blockchain.Blockchain();
            Console.WriteLine("Address: " + opts.Address);
            blockchain.Init(new byte[20]);
        }
	}
	
}
