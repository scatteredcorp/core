using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Schema;
using BGC.Marbles;
using Secp256k1Net;

namespace BGC.Contracts {

    public interface IContract {
        byte[] Serialize(ContractHelper.SerializationType serializationType);
        bool Validate();
        
        byte GetType();
        
        bool Sign(byte[] privateKey, uint nonce);

        void PrettyPrint();
    }

    // Defines a contract with multiple signatures such as a Transaction or Start contract
    public interface IContractMultiSig : IContract {
        bool PartialSign(byte[] privateKey, uint playerOneNonce);
    }

    public enum ContractType {
        StartContract,
        ThrowContract,
        TransactionContract
    }

    public static class ContractHelper {
        
        public enum SerializationType {
            NoSig,
            Partial,
            Complete
        }

        public static IContract[] DeserializeMany(uint count, byte[] contractsBytes) {
            List<IContract> contracts = new List<IContract>();
            uint offset = 0;
            for (uint i = 0; i < count; i++) {
                (IContract contract, uint size) = DeserializeWithSize(contractsBytes, offset);
                contracts.Add(contract);
                offset += size;
            }

            return contracts.ToArray();
        }
        
        public static IContract Deserialize(byte[] bytes) {
            (IContract contract, _) = DeserializeWithSize(bytes);
            return contract;
        }
        
        public static (IContract, uint) DeserializeWithSize(byte[] contract, uint offset = 0) {
            byte contractType = contract[1];
            IContract result;
            uint size;
            switch (contractType) {
                case (byte) ContractType.StartContract:
                    (result, size) = DeserializeStartContract(contract, offset);
                    break;
                
                case (byte) ContractType.ThrowContract:
                    (result, size) = DeserializeThrowContract(contract, offset);
                    break;

                case (byte) ContractType.TransactionContract:
                    (result, size) = DeserializeTransactionContract(contract, offset);
                    break;
                
                default:
                    throw new InvalidDataException("Contract type is not valid.");
            }

            return (result, size);
        }

        public static bool PartialSign(IContractMultiSig contractMultiSig, byte[] privateKey, uint nonce) {
            return contractMultiSig.PartialSign(privateKey, nonce);
        }

        public static bool Sign(IContract contract, byte[] privateKey, uint nonce) {
            return contract.Sign(privateKey, nonce);
        }

        private static Placement DeserializePlacement(byte[] contract, ref uint offset) {
            Placement placement = new Placement();
            byte numMarbles = contract[offset];
            offset++;
            for (uint i = 0; i < numMarbles; i++) {
                byte type = contract[offset];
                offset++;

                byte[] quantityBytes = new byte[4];
                Array.Copy(contract, offset, quantityBytes, 0, 4);
                uint quantity = BitConverter.ToUInt32(quantityBytes);
            
                placement.Add(type, quantity);
                offset += 4;
            }

            return placement;
        }

        private static byte[] DeserializeAddress(byte[] data, ref uint offset) {
            byte[] pubKeyHash = new byte[25];
            Array.Copy(data, offset, pubKeyHash, 0, 25);
            
            offset += 25;
            return pubKeyHash;
        }

        private static uint DeserializeNonce(byte[] data, ref uint offset) {
            byte[] nonceBytes = new byte[4];
            Array.Copy(data, offset, nonceBytes, 0, 4);
            
            offset += 4;
            return BitConverter.ToUInt32(nonceBytes);;
        }

        private static byte[] DeserializeSignature(byte[] data, ref uint offset) {
            byte[] signature = new byte[Secp256k1.UNSERIALIZED_SIGNATURE_SIZE];
            Array.Copy(data, offset, signature, 0, 65);
            offset += Secp256k1.UNSERIALIZED_SIGNATURE_SIZE;
            return signature;
        }

        private static byte[] DeserializeHash(byte[] data, ref uint offset) {
            byte[] hash = new byte[32];
            Array.Copy(data, offset, hash, 0, 32);
            offset += 32;
            return hash;
        }
        
        private static (StartContract, uint) DeserializeStartContract(byte[] data, uint offset = 0) {
            // Contract version
            byte version = data[offset];
            offset++;
            
            // Contract type
            byte contractType = data[1];
            offset++;
            
            // Fee
            Placement fee = DeserializePlacement(data, ref offset);

            // Player one placement
            Placement playerOnePlacement= DeserializePlacement(data, ref offset);

            // Player two placement
            Placement playerTwoPlacement = DeserializePlacement(data, ref offset);

            // Player one pubkey hash
            byte[] pKeyHashOne = DeserializeAddress(data, ref offset);

            // Player two pubkey hash
            byte[] pKeyHashTwo = DeserializeAddress(data, ref offset);

            // Player two nonce
            uint playerOneNonce = DeserializeNonce(data, ref offset);

            if (offset == data.Length) {
                // NoSig deserialization
                return (new StartContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, 0, null, null), offset);
            }
            
            // Signature player one
            byte[] signatureOne = DeserializeSignature(data, ref offset);

            // Player one nonce
            uint playerTwoNonce = DeserializeNonce(data, ref offset);
            
            if (offset == data.Length) {
                // PartialSign deserialization
                return (new StartContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, playerTwoNonce, signatureOne, null), offset);
            }
            
            // Signature player two
            byte[] signatureTwo = DeserializeSignature(data, ref offset);
            
            StartContract contract = new StartContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, playerTwoNonce, signatureOne, signatureTwo);
            return (contract, offset);
        }

        private static (ThrowContract, uint) DeserializeThrowContract(byte[] data, uint offset = 0) {
            byte version = data[offset];
            offset++;
            
            byte contractType = data[offset];
            offset++;

            Placement fee = DeserializePlacement(data, ref offset);
            
            byte x = data[offset];
            offset++;
            byte z = data[offset];
            offset++;

            byte[] gameHash = DeserializeHash(data, ref offset);
            byte throwNonce = data[offset];
            offset++;
            
            uint nonce = DeserializeNonce(data, ref offset);
            
            if (offset == data.Length) {
                // NoSig deserialization
                return (new ThrowContract(fee, gameHash, x, z, throwNonce, nonce), offset);
            }
            byte[] signature = DeserializeSignature(data, ref offset);
            
            return (new ThrowContract(fee, gameHash, x, z, throwNonce, nonce, signature), offset);
        }

        private static (TransactionContract, uint) DeserializeTransactionContract(byte[] data, uint offset = 0) {
            // Contract version
            byte version = data[offset];
            offset++;
            
            // Contract type
            byte contractType = data[1];
            offset++;
            
            // Fee
            Placement fee = DeserializePlacement(data, ref offset);

            // Player one placement
            Placement playerOnePlacement= DeserializePlacement(data, ref offset);

            // Player two placement
            Placement playerTwoPlacement = DeserializePlacement(data, ref offset);

            // Player one pubkey hash
            byte[] pKeyHashOne = DeserializeAddress(data, ref offset);

            // Player two pubkey hash
            byte[] pKeyHashTwo = DeserializeAddress(data, ref offset);

            // Player two nonce
            uint playerOneNonce = DeserializeNonce(data, ref offset);

            if (offset == data.Length) {
                // NoSig deserialization
                return (new TransactionContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, 0, null, null), offset);
            }
            
            // Signature player one
            byte[] signatureOne = DeserializeSignature(data, ref offset);

            // Player one nonce
            uint playerTwoNonce = DeserializeNonce(data, ref offset);
            
            if (offset == data.Length) {
                // PartialSign deserialization
                return (new TransactionContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, playerTwoNonce, signatureOne, null), offset);
            }
            
            // Signature player two
            byte[] signatureTwo = DeserializeSignature(data, ref offset);
            
            TransactionContract contract = new TransactionContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, playerTwoNonce, signatureOne, signatureTwo);
            return (contract, offset);
        }
    }
}
