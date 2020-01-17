using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BGC.Marbles;

namespace BGC.Contracts {

    public class StartContract : BaseContract {
        private new const byte Version = 1;
        
        public Placement PlayerOnePlacement;
        public Placement PlayerTwoPlacement;

        public byte[] PlayerOnePubKeyHash;
        public byte[] PlayerTwoPubKeyHash;
        
        public byte[] PlayerOneSignature;
        public byte[] PlayerTwoSignature;

        public StartContract(Placement fee, ulong nonce, Placement playerOnePlacement, Placement playerTwoPlacement, byte[] playerOnePubKeyHash,
            byte[] playerTwoPubKeyHash) : base(fee, nonce) {
            PlayerOnePlacement = playerOnePlacement;
            PlayerTwoPlacement = playerTwoPlacement;

            if (playerOnePubKeyHash.Length != 20 || playerTwoPubKeyHash.Length != 20) {
                throw new Exception("Public key hash should be 20 bytes.");
            }
            
            PlayerOnePubKeyHash = playerOnePubKeyHash;
            PlayerTwoPubKeyHash = playerTwoPubKeyHash;
            
            PlayerOneSignature = new byte[32];
            PlayerTwoSignature = new byte[32];
        }

        public byte[] Serialize() {
            var serialized = new List<byte>();
            serialized.Add(Version);
            serialized.Add(0); // contract type is 0
            
            serialized.AddRange(Fee.Serialize());
            
            serialized.AddRange(PlayerOnePubKeyHash);
            serialized.AddRange(PlayerTwoPubKeyHash);
            
            serialized.AddRange(PlayerOnePlacement.Serialize());
            serialized.AddRange(PlayerTwoPlacement.Serialize());
            
            serialized.AddRange(BitConverter.GetBytes(Nonce));
            
            serialized.AddRange(PlayerOneSignature);
            serialized.AddRange(PlayerTwoSignature);
            
            return serialized.ToArray();
        }

        public bool Sign(byte[] privateKey) {
            throw new NotImplementedException();
        }
    }
}
