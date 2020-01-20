using System;
using CommandLine;

namespace BGC.CLI {
	public static class WalletCmd {
		[Verb("createwallet", HelpText = "Create a new wallet")]
		public class CreateWalletOptions { }

		public static void CreateWallet(CreateWalletOptions opts) {
			Console.WriteLine("Create a new wallet.");
			throw new NotImplementedException();
		}

		[Verb("getwallet", HelpText = "Return public wallet address")]
		public class GetWalletOptions { }
		public static void GetWallet(GetWalletOptions opts) {
			throw new NotImplementedException();
		}
		
	}
}
