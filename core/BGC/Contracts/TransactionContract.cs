﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using BGC.Marbles;

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
    /// 64 bytes : Player one signature
    ///
    /// 4 bytes : Player two nonce
    /// 64 bytes : Player two signature
    /// </summary>
    public class TransactionContract : IContract {
        public const byte Version = 1;
        public const byte Type = (byte) ContractType.TransactionContract;

        public Placement Fee { get; }
        
        public Placement PlayerOnePlacement { get; }
        public Placement PlayerTwoPlacement { get; }

        public byte[] PlayerOnePubKeyHash { get; }
        public byte[] PlayerTwoPubKeyHash { get; }
        

        public uint PlayerOneNonce { get; }
        public uint PlayerTwoNonce { get; }
        
        public byte[] PlayerOneSignature { get; }
        public byte[] PlayerTwoSignature { get; }

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
            
            PlayerOneSignature = playerOneSignature ?? new byte[64];
            PlayerTwoSignature = playerTwoSignature ?? new byte[64];
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

        public void PrettyPrint() {
            throw new NotImplementedException();
        }

        public byte[] PartialSerialize() {
            var serialized = new List<byte>();
            
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
            // Append player one signature
            serialized.AddRange(PlayerOneSignature);
            
            return serialized.ToArray();
        }

        public byte[] Serialize() {
            List<byte> partial = PartialSerialize().ToList();
            
            // Append player two nonce
            partial.AddRange(BitConverter.GetBytes(PlayerTwoNonce));
            // Append player two signature
            partial.AddRange(PlayerTwoSignature);

            return partial.ToArray();
        }

        public byte[] PartialSign() {
            throw new NotImplementedException();
        }
        
        public bool Sign(byte[] privateKey) {
            throw new NotImplementedException();
        }
        public new byte GetType() {
            return Type;
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
            
            TransactionContract coinbase = new TransactionContract(empty,empty, reward, new byte[25], minerAddress);
            return coinbase;
        }
    }

}