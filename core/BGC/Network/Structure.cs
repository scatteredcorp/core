using System;
using System.Collections.Generic;
using System.Text;

namespace BGC.Network
{
    public class Message
    {    
        // Basic structure minimal size
        public static UInt32 MessageStructureSize = sizeof(MAGIC) + sizeof(COMMAND)
            + 4     // Length size: Uint32 (4x bytes)
            + 4;    // Checksum size: First 4 bytes of SHA256

        // Message Part 1: MAGIC
        // SIZE: 1 BYTE
        // Indicates origin network
        public enum MAGIC : byte
        {
            MainNetwork = 1,
            TestNetwork = 2,
            LocalNetwork = 3
        }

        // Message Part 2: COMMAND
        // SIZE: 1 BYTE
        // Indicates the message's content
        public enum COMMAND : byte
        {
            GetBlock = 1,
            SendBlock = 2,

            GetVersion = 3,
            SendVersion = 4,

            GetHeaders = 5,
            SendHeaders = 6,

            GetMempool = 7,
            SendMempool = 8,

            GetContracts = 9,

            TextMessage = 10,

            SendContract,

            GetInventory,
            SendInventory,

            GetPublicKey,
            SendPublicKey,
            
            RegisterNode,

            UnityGetInventory,
            UnitySendInventory,
            UnityGetWallet,
            UnitySendWallet,
            UnityGetOwnInventory,
            UnitySendOwnInventory
        }
    }
}
