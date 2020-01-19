using System;
using System.Collections.Generic;

namespace BGC.Contracts {
    
    
    public class ThrowContract : BaseContract, IContract {
        private new const byte Version = 1;
        private new const byte Type = 1;

        public byte[] LastThrowHash;
        public byte X;
        public byte Z;
        public byte[] Signature;


        public ThrowContract(Placement fee, ulong nonce, byte[] lastThrowHash, byte x, byte z) : base(fee, nonce) {
            
            base.Version = Version;
            base.Type = Type;
            
            LastThrowHash = lastThrowHash;
            X = x;
            Z = z;
            
            Signature = new byte[64];
        }
        
        public byte[] Serialize() {
            List<byte> serialized = new List<byte>();
            serialized.Add(Version);
            serialized.Add(Type);
            
            serialized.AddRange(Fee.Serialize());
            serialized.Add(X);
            serialized.Add(Z);
            
            serialized.AddRange(LastThrowHash);
            serialized.AddRange(BitConverter.GetBytes(Nonce));

            serialized.AddRange(Signature);
            
            return serialized.ToArray();
        }

        public bool Sign(byte[] privateKey) {

            throw new NotImplementedException();
            
        }
        
    }
    
    
}