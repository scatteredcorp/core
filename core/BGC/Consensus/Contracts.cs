using System;
using System.Security.Cryptography;
using BGC.Contracts;
using Secp256k1Net;

namespace BGC.Consensus {
    public static class Contracts {
        public static bool ValidateSignature(IContract contract, byte[] signature, bool multiSig = false) {
            byte[] data = contract.Serialize(ContractHelper.SerializationType.NoSig);
            SHA256 sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(data);
            
            using (Secp256k1 secp256k1 = new Secp256k1()) {
                byte[] pubKey = new byte[64];
                secp256k1.Recover(pubKey, signature, hash);

                byte[] normalizedSig = new byte[64];
                secp256k1.SignatureNormalize(normalizedSig, signature);
                
                bool valid = secp256k1.Verify(normalizedSig, hash, pubKey);
                Console.WriteLine("is valid = " + valid);
                return valid;
            }
        }
    }
}