using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using BGC.Marbles;

namespace BGC.Contracts {

    public class StartContract : BaseContract, IContract {
        private new const byte Version = 1;
        private new const byte Type = 0;

        public Placement PlayerOnePlacement;
        public Placement PlayerTwoPlacement;

        public byte[] PlayerOnePubKeyHash;
        public byte[] PlayerTwoPubKeyHash;
        
        public byte[] PlayerOneSignature;
        public byte[] PlayerTwoSignature;

        public StartContract(Placement fee, ulong nonce, Placement playerOnePlacement, Placement playerTwoPlacement, byte[] playerOnePubKeyHash,
            byte[] playerTwoPubKeyHash) : base(fee, nonce) {

            base.Version = Version;
            base.Type = Type;
            
            PlayerOnePlacement = playerOnePlacement;
            PlayerTwoPlacement = playerTwoPlacement;

            if (playerOnePubKeyHash.Length != 25 || playerTwoPubKeyHash.Length != 25) {
                throw new Exception("Public key hash should be 25 bytes.");
            }
            
            PlayerOnePubKeyHash = playerOnePubKeyHash;
            PlayerTwoPubKeyHash = playerTwoPubKeyHash;
            
            PlayerOneSignature = new byte[64];
            PlayerTwoSignature = new byte[64];
        }

        public byte[] Serialize() {
            var serialized = new List<byte>();
            serialized.Add(Version);
            serialized.Add(Type);
            
            serialized.AddRange(Fee.Serialize());
            
            serialized.AddRange(PlayerOnePlacement.Serialize());
            serialized.AddRange(PlayerTwoPlacement.Serialize());
            
            serialized.AddRange(PlayerOnePubKeyHash);
            serialized.AddRange(PlayerTwoPubKeyHash);
            
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