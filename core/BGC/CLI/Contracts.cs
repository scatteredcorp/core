using System;
using System.Text;
using BGC.Base58;
using CommandLine;
using BGC.Contracts;
using BGC.Wallet;

namespace BGC.CLI {
	public static class ContractsCmd {
		[Verb("signcontract", HelpText = "Sign a contract")]
		public class SignContractOptions {
			[Value(0)]
			public string Contract { get; set; }
			[Value(1)]
			public string Password { get; set; }
		}
		
		public static void SignContract(SignContractOptions opts) {
			byte[] contractBytes = Utils.FromHex(opts.Contract);
			IContract contract = ContractHelper.Deserialize(contractBytes);

			byte[] pass = Utils.StringToBytes(opts.Password);

			Wallet.Wallet wallet = WalletHelper.LoadWallet();

			// TODO change nonce dynamically
			contract.Sign(wallet.PrivateKey, 1337);

			byte[] bytes = contract.Serialize(ContractHelper.SerializationType.Complete);
			
		}
		
		[Verb("decodecontract", HelpText = "Decode a contract")]
		public class DecodeContractOptions {
			[Value(0)]
			public string Contract { get; set; }
		}
		
		public static void DecodeContract(DecodeContractOptions opts) {
			byte[] contractBytes = Utils.FromHex(opts.Contract);
			IContract contract = ContractHelper.Deserialize(contractBytes);
			contract.PrettyPrint();
		}
	}
	
}
