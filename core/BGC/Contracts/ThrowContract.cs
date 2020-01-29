using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using BGC.Base58;

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
    /// 1 byte : Throw Nonce (Sequence of throws in game)
    /// 
    /// 4 bytes : Nonce (Player nonce to prevent double-spend)
    /// 64 bytes : Signature
    /// </summary>
    public class ThrowContract : IContract {
        public const byte Version = 1;
        public const byte Type = (byte) ContractType.ThrowContract;

        public Placement Fee { get; }
        
        public byte X { get; }
        public byte Z { get; }
        
        public byte[] GameHash { get; }

        public byte ThrowNonce { get; }
        public uint Nonce { get; private set; }
        public byte[] Signature { get; private set; }

        public ThrowContract(Placement fee, byte[] gameHash, byte x, byte z, byte throwNonce, uint nonce = 0, byte[] signature = null) {

            Fee = fee;
            
            X = x;
            Z = z;

            GameHash = gameHash;
            
            ThrowNonce = throwNonce;
            Nonce = nonce;
            
            Signature = signature ?? new byte[64];
        }

        public bool Validate() {
            throw new NotImplementedException();
        }

        public void PrettyPrint() {
            Console.WriteLine("Version: {0}", Version);
            Console.WriteLine("Type: {0}", Type);
            Console.WriteLine("Fee:");
            Fee.PrettyPrint();
            
            Console.WriteLine("X: {0}", X);
            Console.WriteLine("Z: {0}", Z);
            
            Console.WriteLine("GameHash: {0}", Utils.ToHex(GameHash));
            
            Console.WriteLine("Throw nonce: {0}", ThrowNonce);
            Console.WriteLine("Player nonce: {0}", Nonce);
            
            Console.WriteLine("Signature: {0}", string.Join(" ", Signature));
        }

        public byte[] Serialize(ContractHelper.SerializationType serializationType = ContractHelper.SerializationType.Complete) {
            if(serializationType == ContractHelper.SerializationType.Partial) throw new Exception("not a multi-signature contract");
            
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
            
            // Append throw nonce
            serialized.Add(ThrowNonce);
            
            // Append nonce
            serialized.AddRange(BitConverter.GetBytes(Nonce));

            if (serializationType == ContractHelper.SerializationType.NoSig) return serialized.ToArray();
            
            // Append signature
            serialized.AddRange(Signature);
            
            return serialized.ToArray();
        }

        public bool Sign(byte[] privateKey, uint nonce) {
            Nonce = nonce;
            byte[] serialized = Serialize(ContractHelper.SerializationType.NoSig);
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
    }
}