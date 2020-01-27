using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using BGC.Marbles;

namespace BGC.Contracts {

    /// <summary>
    /// Serialization:
    /// 1 byte : Version
    /// 1 byte : Type
    
    /// ? bytes : Fee (Placement)
    /// ? bytes : Player one placement
    /// ? bytes : Player two placement
    ///
    /// 25 bytes : Player one pubkey hash (address)
    /// 25 bytes : Player two pubkey hash (address)
    ///
    /// 4 bytes : Player one nonce
    /// 64 bytes : Player one signature
    ///
    /// 4 bytes : Player two nonce
    /// 64 bytes : Player two signature 
    /// </summary>
    public class StartContract : IContractMultiSig {
        public const byte Version = 1;
        public const byte Type = (byte) ContractType.StartContract;
        
        // Player One pays for fee
        public Placement Fee { get; }
        
        public Placement PlayerOnePlacement { get; }
        public Placement PlayerTwoPlacement { get; }

        public byte[] PlayerOnePubKeyHash { get; }
        public byte[] PlayerTwoPubKeyHash { get; }

        public uint PlayerOneNonce { get; private set; }
        public uint PlayerTwoNonce { get; private set; }

        public byte[] PlayerOneSignature { get; private set; }
        public byte[] PlayerTwoSignature { get; private set; }

        public StartContract(
            Placement fee, 
            Placement playerOnePlacement, 
            Placement playerTwoPlacement, 
            byte[] playerOnePubKeyHash, 
            byte[] playerTwoPubKeyHash, 
            uint playerOneNonce = 0, 
            uint playerTwoNonce = 0, 
            byte[] playerOneSig = null, 
            byte[] playerTwoSig = null
        ) {

            Fee = fee;
            
            PlayerOnePlacement = playerOnePlacement;
            PlayerTwoPlacement = playerTwoPlacement;
            
            PlayerOnePubKeyHash = playerOnePubKeyHash;
            PlayerTwoPubKeyHash = playerTwoPubKeyHash;
            
            PlayerOneNonce = playerOneNonce;
            PlayerTwoNonce = playerTwoNonce;

            PlayerOneSignature = playerOneSig;
            PlayerOneSignature = playerTwoSig;
        }

        public bool Validate() {
            // TODO Validate contract deeper
            if (!Utils.ValidateAddress(PlayerOneSignature) || !Utils.ValidateAddress(PlayerTwoPubKeyHash)) {
                return false;
            }

            return true;
        }

        public byte[] Serialize(ContractHelper.SerializationType serializationType = ContractHelper.SerializationType.Complete) {
            List<byte> serialized = new List<byte>();
            // Append contract version
            serialized.Add(Version);
            
            // Append contract type
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

            // If serialization type is no sig, return unsigned contract
            if (serializationType == ContractHelper.SerializationType.NoSig) return serialized.ToArray();

            if (PlayerOneSignature == null) throw new Exception("Contract not signed.");
            
            // Append player one signature
            serialized.AddRange(PlayerOneSignature);
            
            // Append player two nonce
            serialized.AddRange(BitConverter.GetBytes(PlayerTwoNonce));
            
            // If serialization type is partial, we keep player one signature and player two so that Player Two can sign Player One Signature
            if (serializationType == ContractHelper.SerializationType.Partial) return serialized.ToArray();
            
            if (PlayerTwoSignature == null) throw new Exception("Contract not signed.");

            // Append player two signature
            serialized.AddRange(PlayerTwoSignature);
            
            // This is the final signed contract ready to be broadcast on the network
            if (serializationType == ContractHelper.SerializationType.Complete) return serialized.ToArray();

            throw new ArgumentException("Serialization type does not exist.");
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
    }

}