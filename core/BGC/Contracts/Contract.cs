using System;
using BGC.Marbles;

namespace BGC.Contracts {

    public interface IContract {
        byte[] Serialize();
        bool Sign();
    }

    public static class ContractHelper {
        public static StartContract DeserializeStartContract(byte[] data) {
            // Contract version
            byte version = data[0];
            
            // Contract type
            byte contractType = data[1];
            byte numMarbles = data[2];
            uint offset = 3;

            Placement fee = new Placement();
            Placement playerOnePlacement = new Placement();
            Placement playerTwoPlacement = new Placement();

            // Fee
            for (uint i = 0; i < numMarbles; i++) {
                byte type = data[offset];
                offset++;

                byte[] quantityBytes = new byte[8];
                Array.Copy(data, offset, quantityBytes, 0, 8);
                ulong quantity = BitConverter.ToUInt64(quantityBytes);
            
                fee.Add(type, quantity);
                offset += 8;
            }
            
            // Player one placement
            numMarbles = data[offset];
            offset++;
            for (uint i = 0; i < numMarbles; i++) {
                byte type = data[offset];
                offset++;

                byte[] quantityBytes = new byte[8];
                Array.Copy(data, offset, quantityBytes, 0, 8);
                ulong quantity = BitConverter.ToUInt64(quantityBytes);

                playerOnePlacement.Add(type, quantity);
                offset += 8;
            }
            
            // Player two placement
            numMarbles = data[offset];
            offset++;
            for (uint i = 0; i < numMarbles; i++) {
                byte type = data[offset];
                offset++;

                byte[] quantityBytes = new byte[8];
                Array.Copy(data, offset, quantityBytes, 0, 8);
                ulong quantity = BitConverter.ToUInt64(quantityBytes);

                playerTwoPlacement.Add(type, quantity);
                offset += 8;
            }

            // Player one pubkey hash
            byte[] pKeyHashOne = new byte[20];
            Array.Copy(data, offset, pKeyHashOne, 0, 20);
            
            offset += 20;

            // Player two pubkey hash
            byte[] pKeyHashTwo = new byte[20];
            Array.Copy(data, offset, pKeyHashTwo, 0, 20);

            offset += 20;
            
            // Nonce
            byte[] nonceBytes = new byte[8];
            Array.Copy(data, offset, nonceBytes, 0, 8);
            ulong nonce = BitConverter.ToUInt64(nonceBytes);

            offset += 8;

            // Signature player one
            byte[] signatureOne = new byte[64];
            Array.Copy(data, offset, signatureOne, 0, 64);
            byte[] playerOneSignature = signatureOne;

            offset += 64;
            
            // Signature player two
            byte[] signatureTwo = new byte[64];
            Array.Copy(data, offset, signatureTwo, 0, 64);

            byte[] playerTwoSignature = signatureTwo;
            
            return new StartContract(fee, nonce, playerOnePlacement, playerTwoPlacement, pKeyHashOne, pKeyHashTwo);
            
        }
    }
    
    public class BaseContract {
        public byte Version;
        public byte Type;
        
        public Placement Fee;
        public ulong Nonce;

        protected BaseContract(Placement fee, ulong nonce) {
            Fee = fee;
            Nonce = nonce;
        }
    }
}
