using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BGC.Base58;
using BGC.Marbles;
using Secp256k1Net;

namespace BGC.Wallet
{
    public class Wallet {

        private byte Version = 0x8a;

        public byte[] PrivateKey { get; }
        public byte[] PublicKey { get; }

        public Wallet(byte[] privateKey) {
            PrivateKey = privateKey;
            PublicKey = WalletHelper.ComputePubKey(privateKey);
        }

        public void SetVersionByte(byte version) {
            Version = version;
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
            byte[] versioned = new byte[ripemdHash.Length + 1];
            versioned[0] = Version;
            
            for (int i = 0; i < ripemdHash.Length; i++)
            {
                versioned[i + 1] = ripemdHash[i];
            }

            // Step 5 & 6: SHA256 twice
            hash = sha526.ComputeHash(versioned);
            hash = sha526.ComputeHash(hash);
            
            // Step 7 & 8: add trimmed hash at the end of the versioned hash
            byte[] address = new byte[ripemdHash.Length + 5];
            {
                int i = 0;
                for (; i < versioned.Length; i++)
                {
                    address[i] = versioned[i];
                }
                for (int j = 0; j < 4; j++)
                {
                    address[i + j] = hash[j];
                }
            }

            return address;
        }
        
        public List<Marbles.Marble> GetInventory() {
            return null;
        }

        public string EncodedAddress() {
            // Return base58 encoded version of Wallet.Address()

            return Base58Encode.Encode(Address());
        }
    }

    public static class WalletHelper {

        private const string WalletPath = "./wallet.dat";

        public static void DeleteWallet() {
            File.Delete(WalletPath);
        }

        public static Contracts.Placement GetInventory(byte[] address) {
			byte[] data = Blockchain.Blockchain.DB.Get(Utils.BuildKey("i", address));
			if(data == null) {
				Console.WriteLine("Player doesn't exist.");
				return null;
			}
			return Contracts.ContractHelper.DeserializePlacement(data);
        }
        
        public static byte[] GeneratePrivateKey() {
            using (var secp256k1 = new Secp256k1()) {
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
                byte[] publicKey = new byte[64];
                secp256k1.PublicKeyCreate(publicKey, privateKey);

                byte[] serialized = new byte[65];
                secp256k1.PublicKeySerialize(serialized, publicKey, Flags.SECP256K1_EC_UNCOMPRESSED);
                return serialized;
            }
        }

        // Compressed public key (only X value, because Y can be derived from X)
        // Leading 0x02 if Y is even
        // Leading 0x03 if Y is odd
        public static byte[] ComputeCompressedPubKey(BigInteger privateKey) {
            throw new NotImplementedException();
        }

        public static bool Exists() {
            return File.Exists(WalletPath);
        }
        
        public static Wallet CreateWallet(byte[] encryptionKey) {
            // Generate private key / public key pair and save in wallet.dat
            // Encrypt wallet.dat with encrpytion key
            // Do not create wallet if wallet.dat exists

            if (Exists()) {
                throw new Exception("wallet already exists");
            }
             
            // Step 1: Generate a private key
            byte[] privateKey = GeneratePrivateKey();

            // // Step 2: Initialize a wallet with that private key
            Wallet wallet = new Wallet(privateKey);

            // byte[] encrypted = Utils.EncryptBytes(privateKey, encryptionKey, new byte[16]);
            
            File.WriteAllBytes(WalletPath, privateKey);

            return wallet;
        }

        public static Wallet LoadWallet() {

            if (!Exists()) {
                throw new FileNotFoundException("wallet does not exist");
            }
            
            byte[] encrypted = File.ReadAllBytes(WalletPath);
            // byte[] decrypted;
            // try {
            //     decrypted = Utils.DecryptBytes(encrypted, encryptionKey, new byte[16]);
            // }
            // catch {
            //     throw new ArgumentException("Decryption key is invalid.");
            // }
            
            return new Wallet(encrypted);
            
        }
    }
}
