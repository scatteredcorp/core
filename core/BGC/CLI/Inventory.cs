using System;
using CommandLine;

namespace BGC.CLI {
	public class InventoryCmd {
		
		[Verb("inventory", HelpText = "Display list of owned marbles")]
		public class InventoryOptions {
			[Option(Group = "address", HelpText="A player's address")]
			public string Address { get; set; }
		}
		public static void GetInventory(InventoryOptions opts) {
			Blockchain.Blockchain.Resume();
			byte[] addr = Base58.Base58Encode.Decode(opts.Address);

			Contracts.Placement inventory = Wallet.WalletHelper.GetInventory(addr);
			inventory.PrettyPrint();
		}
	}
}
