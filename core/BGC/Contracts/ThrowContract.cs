using System;
using System.Collections.Generic;

namespace BGC.Contracts {
    
    
    /// <summary>
    /// Serialization:
    /// 1 byte : Version
    /// 1 byte : Type
    ///
    /// ? bytes : Fee (Placement)
    ///
    /// 1 byte : Vector.X
    /// 1 byte : Vector.Z
    ///
    /// 32 bytes : Game Hash (StartContract hash)
    ///
    /// 4 bytes : Nonce
    /// 64 bytes : Signature
    /// </summary>
    public class ThrowContract : IContract {
        public const byte Version = 1;
        public const byte Type = (byte) ContractType.ThrowContract;

        public Placement Fee { get; }
        
        public byte[] GameHash { get; }
        public byte X { get; }
        public byte Z { get; }
        
        public uint Nonce { get; }
        public byte[] Signature { get; }

        public ThrowContract(Placement fee, byte[] gameHash, byte x, byte z, uint nonce = 0, byte[] signature = null) {

            Fee = fee;
            
            GameHash = gameHash;
            X = x;
            Z = z;
            
            Nonce = nonce;
            Signature = signature ?? new byte[64];
        }

        public byte[] Serialize(ContractHelper.SerializationType serializationType = ContractHelper.SerializationType.Complete) {
            throw new NotImplementedException();
        }

        public bool Validate() {
            throw new NotImplementedException();
        }

        public bool Sign(byte[] privateKey, uint nonce) {
            throw new NotImplementedException();
        }

        public byte[] Serialize() {
            List<byte> serialized = new List<byte>();
            
            // Append version
            serialized.Add(Version);
            // Append type
            serialized.Add(Type);
            
            // Append fee
            serialized.AddRange(Fee.Serialize());
            
            // Append throw vector
            serialized.Add(X);
            serialized.Add(Z);
            
            // Append game hash (StartContract hash)
            serialized.AddRange(GameHash);
            
            // Append nonce
            serialized.AddRange(BitConverter.GetBytes(Nonce));

            // Append signature
            serialized.AddRange(Signature);
            
            return serialized.ToArray();
        }

        public bool Sign(byte[] privateKey) {
            throw new NotImplementedException();
        }
    }
}