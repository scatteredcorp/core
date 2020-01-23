using System;
using BGC.Base58;
using CommandLine;
using BGC.Contracts;
namespace BGC.CLI {
	public static class ContractsCmd {
		[Verb("signcontract", HelpText = "Sign a contract")]
		public class ContractOptions {
			[Value(0)]
			public string Contract { get; set; }
		}
		
		public static void SignContract(ContractOptions opts) {
			Console.WriteLine(opts.Contract);
		}
	}
	
}
