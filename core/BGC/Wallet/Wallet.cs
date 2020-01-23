using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BGC.Base58;
using Secp256k1Net;

namespace BGC.Wallet
{
    public class Wallet {

        private const byte Version = 1;
        
        public byte[] PrivateKey;
        public byte[] PublicKey;

        public Wallet(byte[] privateKey) {
            PrivateKey = privateKey;
            PublicKey = WalletHelper.ComputePubKey(privateKey);
        }

        public byte[] Address() {
            // https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
            
            SHA256 sha526 = SHA256.Create();

            // Step 2: SHA256 hash
            byte[] hash = sha526.ComputeHash(PublicKey);

            // Step 3: RIPEMD-160 hash
            RIPEMD160 ripemd = RIPEMD160Managed.Create();
            byte[] ripemdHash = ripemd.ComputeHash(hash);
            

            // Step 4: Add version byte (0x00)
            byte[] versionned = new byte[ripemdHash.Length + 2];
            versionned[0] = 0;
            versionned[1] = 0;
            for (int i = 0; i < ripemdHash.Length; i++)
            {
                versionned[i + 2] = ripemdHash[i];
            }

            // Step 5 & 6: SHA256 twice
            hash = sha526.ComputeHash(versionned);
            hash = sha526.ComputeHash(hash);

            // Step 7 & 8: add trimmed hash at the end of the versionned hash
            byte[] address = new byte[ripemdHash.Length + 6];
            {
                int i = 0;
                for (; i < versionned.Length; i++)
                {
                    address[i] = versionned[i];
                }
                for (int j = 0; j < 4; j++)
                {
                    address[i + j] = hash[j];
                }
            }

            return address;
        }

        public string EncodedAddress() {
            // Return base58 encoded version of Wallet.Address()

            return Base58Encode.Encode(Address());
        }
    }

    public static class WalletHelper {

        private static byte[] GeneratePrivateKey() {
            using (var secp256k1 = new Secp256k1())
            {
                byte[] privateKey = new byte[32];
                RandomNumberGenerator rnd = 
                    System.Security.Cryptography.RandomNumberGenerator.Create();
                do { rnd.GetBytes(privateKey); }
                while (!secp256k1.SecretKeyVerify(privateKey));
                return privateKey;
            }
        }

        // Uncompressed public key (X and Y value)
        // Leading 0x04 byte (uncompressed format)
        public static byte[] ComputePubKey(byte[] privateKey) {
            using (var secp256k1 = new Secp256k1())
            {
                var publicKey = new byte[64];
                secp256k1.PublicKeyCreate(publicKey, privateKey);
                return publicKey;
            }
        }

        // Compressed public key (only X value, because Y can be derived from X)
        // Leading 0x02 if Y is even
        // Leading 0x03 if Y is odd
        public static byte[] ComputeCompressedPubKey(BigInteger privateKey) {
            throw new NotImplementedException();
        }
        
        public static Wallet CreateWallet(byte[] encryptionKey) {
            // Generate private key / public key pair and save in wallet.dat
            // Encrypt wallet.dat with encrpytion key
            // Do not create wallet if wallet.dat exists
            throw new NotImplementedException();
        }

        public static Wallet LoadWallet() {
            // Load wallet.dat file
            throw new NotImplementedException();
        }
    }
}