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

			byte[] pkhash = Base58.Base58Encode.Decode(opts.Address);
			byte[] data = Blockchain.Blockchain.DB.Get(Utils.BuildKey("i", pkhash));
			if(data == null) {
				Console.WriteLine("Player doesn't exist.");
				return;
			}
			Contracts.Placement inventory = Contracts.ContractHelper.DeserializePlacement(data);
			inventory.PrettyPrint();
		}
	}
}
