using System;
using BGC.Base58;
using CommandLine;
using BGC.Blockchain;

namespace BGC.CLI {
	public static class BlockchainCmd {
		[Verb("init", HelpText = "Initialize blockchain")]
		public class InitOptions {
			[Option(Group = "init", HelpText="Genesis block reward address")]
			public string Address { get; set; } // Base58 address
		}
		
        public static void InitBlockchain(InitOptions opts) {
            if (!Utils.ValidateAddress(opts.Address)) {
	            Console.WriteLine("Address provided is invalid");
	            CLI.Exit(1);
            }
            byte[] address = Base58Encode.Decode(opts.Address);
            Blockchain.Blockchain.Init(address);
            Console.WriteLine("Blockchain successfully initialized!");
        }

		[Verb("showblocks", HelpText = "Show blocks")]
		public class ShowBlocksOptions {
			[Option(Group = "limit", HelpText="How many blocks to print", Default=5)]
			public int Limit { get; set; }
		}
		public static void ShowBlocks(ShowBlocksOptions options) {
			Blockchain.Blockchain.Resume();

			Iterator iterator = new Iterator();
			int i = 0;
			while(iterator.CanIterate && i < options.Limit) {
				Block block = iterator.Next();
				string hash = block.BlockHeader.HashString();
				Console.WriteLine(hash);
				i++;
			}
		}

		[Verb("mine", HelpText = "Mine blocks")]
		public class MineOptions {
			[Option(Group = "address", HelpText="Address to send rewards to")]
			public string Address { get; set; }
		}
		public static void Mine(MineOptions options) {
			Blockchain.Blockchain.Resume();
			StartMining(options);
		}

		private static void StartMining(MineOptions options) {
			var watch = System.Diagnostics.Stopwatch.StartNew();

			BlockHeader header = new BlockHeader(BlockHelper.Version, Blockchain.Blockchain.LastHash, new byte[32], Consensus.Mining.Target());
			
			byte[] address = Base58.Base58Encode.Decode(options.Address);
		 	Contracts.TransactionContract contract =  Contracts.TransactionHelper.CoinbaseTransaction(address);
			
			Block block = new Block(header, new Contracts.IContract[1]{contract});

			ProofOfWork.PoW pow = new ProofOfWork.PoW(block);

			(uint nonce, byte[] hash) = pow.Run();
			block.BlockHeader.Nonce = nonce;
			Blockchain.Blockchain.PushBlock(block);

			Console.WriteLine("Written block on the BGC network.");
			Console.WriteLine(block.BlockHeader.HashString());


			// the code that you want to measure comes here
			watch.Stop();
			var elapsedMs = watch.ElapsedMilliseconds;
			Console.WriteLine("Elapsed: " + elapsedMs/1000 + " seconds");

			StartMining(options);
		}
	}
	
}
