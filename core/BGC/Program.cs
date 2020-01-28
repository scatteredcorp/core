using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using BGC.Base58;
using BGC.Contracts;
using BGC.Wallet;
using Secp256k1Net;

namespace BGC {
    class BGC {
        static void Main(string[] args) {
            CLI.CLI.Run(args);
            
            // Create dummy contract
            Placement fee = new Placement();
            fee.Add(0, 1942);
            fee.Add(1, 155);
            fee.Add(5, 155);
            
            Placement playerOnePlacement = new Placement();
            playerOnePlacement.Add(2, 64);
            playerOnePlacement.Add(4, 32);

            Placement playerTwoPlacement = new Placement();
            playerTwoPlacement.Add(5, 122);
            playerTwoPlacement.Add(8, 2);
            
            // Dummy wallets
            Wallet.Wallet wallet1 = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            Wallet.Wallet wallet2 = new Wallet.Wallet(WalletHelper.GeneratePrivateKey());
            
            // Create contract
            StartContract startContract = new StartContract(fee, playerOnePlacement, playerTwoPlacement,
                wallet1.Address(), wallet2.Address());

            // Serialize it (without signatures)
            byte[] bytes = startContract.Serialize(ContractHelper.SerializationType.NoSig);
            
            // Deserialize it
            StartContract resultContract = (StartContract) Contracts.ContractHelper.Deserialize(bytes);
    
            // Player one signs the contract
            startContract.PartialSign(wallet1.PrivateKey, 0);
            string hex = Utils.ToHex(startContract.Serialize(ContractHelper.SerializationType.Partial));

            string c =
                "0100030096070000019B000000059B000000020240000000042000000002057A000000080200000000CAD2035796D8E2BC42E1D6149430B919A9AAF53F47B3CD4400FCFA23089C63A9EDF34A518007A81A7E2895D450117CF90E0000000043899CC115250151019DF60DB62157E4E10ED8B7A5240E575AC31DEA632C9E47D235E30B00BFC70CB475D4B99A82082D1CEE8509805C2848E71567A93C19A92B3905000072B12BAD31FFDBBE582D294EF251C54D314EFF6EC6FFE1F5B76AECB13D0EC3BD3AF6068DFA7156D311650C7BC0CFC6BF61601BEC192371428E469F3D59510B53";
            byte[] cBytes = Utils.FromHex(c);
            StartContract s = (StartContract) ContractHelper.Deserialize(cBytes);
            s.PrettyPrint();
        }
    }
}