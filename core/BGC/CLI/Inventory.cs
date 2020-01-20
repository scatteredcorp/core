using System;
using CommandLine;

namespace BGC.CLI {
	public class InventoryCmd {
		
		[Verb("inventory", HelpText = "Display list of owned marbles")]
		public class InventoryOptions {
		}
		public static void GetInventory(InventoryOptions opts) {
			throw new NotImplementedException();
		}
	}
}
