using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using BGC.Base58;
using BGC.Marbles;
using BGC.Wallet;

namespace BGC.Contracts {

    /// <summary>
    /// Serialization:
    /// 1 byte : Version
    /// 1 byte : Type
    
    /// ? bytes : Fee (Placement)
    /// 32 byes : Game hash
    ///
    /// 4 bytes : Winner nonce
    /// 65 bytes : Winner signature
    /// </summary>

    public class ClaimContract : IContract {
        public const byte Version = 1;
        public byte Type { get; } = (byte) ContractType.ClaimContract;
        
        // Player One pays for fee
        public Placement Fee { get; }
        public byte[] GameHash {get; }
        public byte[] Address { get; }
        public uint Nonce { get; private set; }

        public byte[] Signature { get; private set; }

        public ClaimContract(
            Placement fee,
            byte[] gameHash,
            byte[] address, 
            uint nonce = 0, 
            byte[] signature = null
        ) {
            Fee = fee;
            GameHash = gameHash;
            Address = address;
            Nonce = nonce;
            Signature = signature;
        }

        public bool Validate() {
            // TODO Validate contract deeper
            if (!Utils.ValidateAddress(Address)) {
                return false;
            }

            if (!Consensus.Contracts.ValidateSignature(this, Signature, false)) return false;

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
            serialized.AddRange(GameHash);
            // Append player two placement
            serialized.AddRange(Address);

            // Append player one nonce
            serialized.AddRange(BitConverter.GetBytes(Nonce));

            // If serialization type is no sig, return unsigned contract
            if (serializationType == ContractHelper.SerializationType.NoSig) return serialized.ToArray();

            if (Signature == null) throw new Exception("Contract not signed.");
            
            // Append player signature
            serialized.AddRange(Signature);
            
            if (serializationType == ContractHelper.SerializationType.Complete) return serialized.ToArray();

            throw new ArgumentException("Serialization type does not exist.");
        }

        public bool Sign(byte[] privateKey, uint nonce) {
            Nonce = nonce;
            byte[] serialized = Serialize(ContractHelper.SerializationType.Partial);
            // Compute signature using serialized byte array
            SHA256 sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(serialized);

            (byte[] sig, bool valid) = Utils.SignData(hash, privateKey);
            if (!valid) {
                return false;
            }

            Signature = sig;
            return true;
        }

        public void PrettyPrint() {
            Console.WriteLine("Version: {0}", Version);
            Console.WriteLine("Type: {0}", Type);
            Console.WriteLine("Fee:");
            Fee.PrettyPrint();
            
            Console.WriteLine("Player address: {0}", Base58Encode.Encode(Address));
            
            Console.WriteLine("Player Nonce: {0}", Nonce);
            Console.WriteLine("Player Sig: {0}", string.Join(" ", Signature));

        }
    }

}