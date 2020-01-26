using System;
using System.IO;
using System.Text;
using BGC.Wallet;
using CommandLine;

namespace BGC.CLI {
	public static class WalletCmd {
        [Verb("createwallet", HelpText = "Create a new wallet")]
        public class CreateWalletOptions {
            [Value(0)]
            public string EncryptionKey { get; set; }
        }

		public static void CreateWallet(CreateWalletOptions opts) {
            if (WalletHelper.Exists()) {
                Console.WriteLine("A wallet already exists.");
                return;
            }
            if (string.IsNullOrEmpty(opts.EncryptionKey)) {
                Console.WriteLine("Please enter a valid encryption key.");
                return;
            }
            byte[] pass = Encoding.UTF8.GetBytes(opts.EncryptionKey);
            Wallet.Wallet wallet = WalletHelper.CreateWallet(pass);
            Console.Write(wallet.EncodedAddress());
		}

        [Verb("getwallet", HelpText = "Return public wallet address")]
        public class GetWalletOptions {
            [Value(0)]
            public string EncryptionKey { get; set; }
        }
		public static void GetWallet(GetWalletOptions opts) {
            if (!WalletHelper.Exists()) {
                Console.WriteLine("Could not find wallet. Please create one.");
                return;
            }
            if (string.IsNullOrEmpty(opts.EncryptionKey)) {
                Console.WriteLine("Please enter a valid decryption key.");
                return;
            }
            byte[] pass = Encoding.UTF8.GetBytes(opts.EncryptionKey);

            Wallet.Wallet wallet = WalletHelper.LoadWallet(pass);
            Console.WriteLine(wallet.EncodedAddress());
        }
        
        [Verb("deletewallet", HelpText = "Delete wallet")]
        public class DeleteWalletOptions { }
        public static void DeleteWallet(DeleteWalletOptions opts) {
            WalletHelper.DeleteWallet();
        }
		
	}
}
