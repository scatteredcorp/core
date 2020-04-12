using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using BGC.Base58;
using BGC.Marbles;
using Secp256k1Net;

namespace BGC.Contracts {

    /// <summary>
    /// Serialization:
    /// 1 byte : Version
    /// 1 byte : Type
    ///
    /// ? bytes : Fee (Placement)
    /// ? bytes : Player one placement
    /// ? bytes : Player two placement
    ///
    /// 25 bytes : Player one pubkey hash (address)
    /// 25 bytes : Player two pubkey hash (address)
    ///
    /// 4 bytes : Player one nonce
    /// 65 bytes : Player one signature
    ///
    /// 4 bytes : Player two nonce
    /// 65 bytes : Player two signature
    /// </summary>
    public class TransactionContract : IContractMultiSig {
        public const byte Version = 1;
        public byte Type { get; } = (byte) ContractType.TransactionContract;

        public Placement Fee { get; }
        
        public Placement PlayerOnePlacement { get; }
        public Placement PlayerTwoPlacement { get; }

        public byte[] PlayerOnePubKeyHash { get; }
        public byte[] PlayerTwoPubKeyHash { get; }
        

        public uint PlayerOneNonce { get; set; }
        public uint PlayerTwoNonce { get; set; }

        public byte[] PlayerOneSignature { get; set; }
        public byte[] PlayerTwoSignature { get; set; }

        public TransactionContract(
            Placement fee, 
            Placement playerOnePlacement, 
            Placement playerTwoPlacement, 
            byte[] playerOnePubKeyHash,
            byte[] playerTwoPubKeyHash,
            
            // Optional
            uint playerOneNonce = 0,
            uint playerTwoNonce = 0,
            byte[] playerOneSignature = null,
            byte[] playerTwoSignature = null
        ) {

            Fee = fee;
            
            PlayerOnePlacement = playerOnePlacement;
            PlayerTwoPlacement = playerTwoPlacement;

            PlayerOnePubKeyHash = playerOnePubKeyHash;
            PlayerTwoPubKeyHash = playerTwoPubKeyHash;
            
            PlayerOneNonce = playerOneNonce;
            PlayerTwoNonce = playerTwoNonce;
            
            PlayerOneSignature = playerOneSignature ?? new byte[Secp256k1.UNSERIALIZED_SIGNATURE_SIZE];
            PlayerTwoSignature = playerTwoSignature ?? new byte[Secp256k1.UNSERIALIZED_SIGNATURE_SIZE];
        }

        public bool Validate() {
            throw new NotImplementedException();
        }

        public bool PartialSign(byte[] privateKey, uint playerOneNonce) {
            PlayerOneNonce = playerOneNonce;
            byte[] serialized = Serialize(ContractHelper.SerializationType.NoSig);
            
            // Compute signature using serialized byte array
            SHA256 sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(serialized);

            (byte[] sig, bool valid) = Utils.SignData(hash, privateKey);
            if (!valid) {
                return false;
            }

            PlayerOneSignature = sig;

            return true;
        }

        public bool Sign(byte[] privateKey, uint playerTwoNonce) {
            PlayerTwoNonce = playerTwoNonce;
            byte[] serialized = Serialize(ContractHelper.SerializationType.Partial);
            // Compute signature using serialized byte array
            SHA256 sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(serialized);

            (byte[] sig, bool valid) = Utils.SignData(hash, privateKey);
            if (!valid) {
                return false;
            }

            PlayerTwoSignature = sig;
            return true;
        }

        public void PrettyPrint() {
            Console.WriteLine("Version: {0}", Version);
            Console.WriteLine("Type: {0}", Type);
            Console.WriteLine("Fee:");
            Fee.PrettyPrint();
            Console.WriteLine("Player One Placement:");
            PlayerOnePlacement.PrettyPrint();
            Console.WriteLine("Player Two Placement:");
            PlayerTwoPlacement.PrettyPrint();
            
            Console.WriteLine("Player One Address: {0}", Base58Encode.Encode(PlayerOnePubKeyHash));
            Console.WriteLine("Player Two Address: {0}", Base58Encode.Encode(PlayerTwoPubKeyHash));
            
            Console.WriteLine("Player One Nonce: {0}", PlayerOneNonce);
            Console.WriteLine("Player One Sig: {0}", string.Join(" ", PlayerOneSignature));
            
            Console.WriteLine("Player Two Nonce: {0}", PlayerTwoNonce);
            Console.WriteLine("Player Two Sig: {0}", string.Join(" ", PlayerTwoSignature));
        }

        public byte[] Serialize(ContractHelper.SerializationType serializationType = ContractHelper.SerializationType.Complete) {
            List<byte> serialized = new List<byte>();
            
            // Append version
            serialized.Add(Version);
            // Append type
            serialized.Add(Type);
            
            // Append fee
            serialized.AddRange(Fee.Serialize());
            
            // Append player one placement
            serialized.AddRange(PlayerOnePlacement.Serialize());
            // Append player two placement
            serialized.AddRange(PlayerTwoPlacement.Serialize());
            
            // Append player one address
            serialized.AddRange(PlayerOnePubKeyHash);
            // Append player two address
            serialized.AddRange(PlayerTwoPubKeyHash);
            
            // Append player one nonce
            serialized.AddRange(BitConverter.GetBytes(PlayerOneNonce));
            
            if (serializationType == ContractHelper.SerializationType.NoSig) return serialized.ToArray();

            // Append player one signature
            serialized.AddRange(PlayerOneSignature);
            
            // Append player two nonce
            serialized.AddRange(BitConverter.GetBytes(PlayerTwoNonce));
            
            if (serializationType == ContractHelper.SerializationType.Partial) return serialized.ToArray();
            
            // Append player two signature
            serialized.AddRange(PlayerTwoSignature);

            return serialized.ToArray();
        }
    }

    public static class TransactionHelper {
        public static TransactionContract CoinbaseTransaction(byte[] minerAddress) {
            uint chainHeight = Blockchain.Blockchain.Height;
            
            Placement reward = new Placement();
            Placement empty = new Placement();
            // TODO : change with actual values based on chain height and marbles' rarity
            reward.Add(0, 1000);
            reward.Add(1, 500);
            reward.Add(2, 100);
            
            TransactionContract coinbase = new TransactionContract(empty,empty, reward, new byte[25], minerAddress);
            return coinbase;
        }
    }

}