using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using BGC.Marbles;

namespace BGC.Contracts {

    public class TransactionContract : BaseContract, IContract {
        private new const byte Version = 1;
        private new const byte Type = 2;
        
        public Placement PlayerOnePlacement;
        public Placement PlayerTwoPlacement;

        public byte[] PlayerOnePubKeyHash;
        public byte[] PlayerTwoPubKeyHash;
        
        public byte[] PlayerOneSignature;
        public byte[] PlayerTwoSignature;

        public TransactionContract(Placement fee, ulong nonce, Placement playerOnePlacement, Placement playerTwoPlacement, byte[] playerOnePubKeyHash,
            byte[] playerTwoPubKeyHash) : base(fee, nonce) {
            PlayerOnePlacement = playerOnePlacement;
            PlayerTwoPlacement = playerTwoPlacement;

            if (playerOnePubKeyHash.Length != 20 || playerTwoPubKeyHash.Length != 20) {
                throw new Exception("Public key hash should be 20 bytes.");
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

    public static class TransactionHelper {
        public static TransactionContract CoinbaseTransaction(byte[] minerAddress, uint chainHeight) {
            Placement reward = new Placement();
            Placement empty = new Placement();
            // TODO : change with actual values based on chain height and marbles' rarity
            reward.Add(0, 1000);
            reward.Add(1, 500);
            reward.Add(2, 100);
            
            TransactionContract coinbase = new TransactionContract(empty, 0, empty, reward, new byte[20], minerAddress);
            return coinbase;
        }
    }

}