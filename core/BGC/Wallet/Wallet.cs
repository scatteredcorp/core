using System;
using System.Numerics;

namespace BGC.Wallet
{
    public class Wallet {

        private const byte Version = 1;
        
        public BigInteger PrivateKey;
        public byte[] PublicKey;

        public Wallet(BigInteger privateKey) {
            PrivateKey = privateKey;
            PublicKey = WalletHelper.ComputePubKey(privateKey);
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

        // Uncompressed public key (X and Y value)
        // Leading 0x04 byte (uncompressed format)
        public static byte[] ComputePubKey(BigInteger privateKey) {
            throw new NotImplementedException();
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