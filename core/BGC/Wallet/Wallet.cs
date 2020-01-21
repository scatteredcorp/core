using System;
using System.Numerics;

namespace BGC.Wallet
{
    public class Wallet {
        public BigInteger PrivateKey;
        public BigInteger PublicKey;

        public Wallet(BigInteger privateKey, BigInteger publicKey) {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public byte[] Address() {
            // https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
            throw new NotImplementedException();
        }

        public string EncodedAddress() {
            // Return base58 encoded version of Wallet.Address()
            throw new NotImplementedException();
        }
    }

    public static class WalletHelper {

        private static BigInteger GeneratePrivateKey() {
            throw new NotImplementedException();
        }

        private static BigInteger ComputePublicKey() {
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