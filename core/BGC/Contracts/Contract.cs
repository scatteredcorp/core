using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Schema;
using BGC.Marbles;

namespace BGC.Contracts {

    public interface IContract {
        byte[] Serialize(ContractHelper.SerializationType serializationType);
        bool Validate();
        //byte Type();
        bool Sign(byte[] privateKey, uint nonce);
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
        
        public static IContract Deserialize(byte[] contract) {
            byte contractType = contract[1];

            switch (contractType) {
                case (byte) ContractType.StartContract:
                    return DeserializeStartContract(contract);
                case (byte) ContractType.ThrowContract:
                    return DeserializeThrowContract(contract);
                case (byte) ContractType.TransactionContract:
                    throw new NotImplementedException();
                default:
                    throw new InvalidDataException("Contract type is not valid.");
            }
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
            byte[] signature = new byte[64];
            Array.Copy(data, offset, signature, 0, 64);
            offset += 64;
            return signature;
        }

        private static byte[] DeserializeHash(byte[] data, ref uint offset) {
            byte[] hash = new byte[32];
            Array.Copy(data, offset, hash, 0, 32);
            offset += 32;
            return hash;
        }
        
        private static StartContract DeserializeStartContract(byte[] data) {
            // Contract version
            byte version = data[0];
            
            // Contract type
            byte contractType = data[1];
            uint offset = 2;

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
                return new StartContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, 0, null, null);
            }
            
            // Signature player one
            byte[] signatureOne = DeserializeSignature(data, ref offset);

            // Player one nonce
            uint playerTwoNonce = DeserializeNonce(data, ref offset);
            
            if (offset == data.Length) {
                // PartialSign deserialization
                return new StartContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, playerTwoNonce, signatureOne, null);
            }
            
            // Signature player two
            byte[] signatureTwo = DeserializeSignature(data, ref offset);
            
            StartContract contract = new StartContract(fee, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo, playerOneNonce, playerTwoNonce, signatureOne, signatureTwo);
            return contract;
        }

        private static ThrowContract DeserializeThrowContract(byte[] data) {
            byte version = data[0];
            byte contractType = data[1];

            uint offset = 2;
            Placement fee = DeserializePlacement(data, ref offset);
            
            byte x = data[offset];
            offset++;
            byte y = data[offset];
            offset++;

            byte[] gameHash = DeserializeHash(data, ref offset);
            uint nonce = DeserializeNonce(data, ref offset);
            byte[] signature = DeserializeSignature(data, ref offset);
            
            return new ThrowContract(fee, gameHash, x, y, nonce, signature);
        }
    }
}
