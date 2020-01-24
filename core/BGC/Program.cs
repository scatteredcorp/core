using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using BGC.Base58;
using BGC.Contracts;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);


            byte[] pass = Encoding.UTF8.GetBytes("Hello guys");
            SHA256 sha = new SHA256Managed();

            byte[] hash = sha.ComputeHash(pass);
            
            Wallet.Wallet wallet = Wallet.WalletHelper.CreateWallet(hash);
            Console.WriteLine(string.Join(" ", wallet.PublicKey));
            
            wallet.SetVersionByte(0x00);
            Console.WriteLine(wallet.EncodedAddress());
            Wallet.Wallet w = Wallet.WalletHelper.LoadWallet(hash);
            w.SetVersionByte(0x00);

            Console.WriteLine(w.EncodedAddress());

        }
    }
}