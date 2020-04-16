using System;
using System.Collections.Generic;
using System.Text;
using BGC.Network;
using System.Net;

namespace BGC.Network
{
    public class NetworkMessage
    {
        public Message.MAGIC magic { get; }
        public Message.COMMAND command { get; }
        byte[] payload { get; }
        IPEndPoint endPoint { get; }
        bool isValid { get; }

        public NetworkMessage(List<byte[]> message, IPEndPoint endPoint)
        {
            this.endPoint = endPoint;

            // Payload size: flex
            UInt32 payloadSize = BitConverter.ToUInt32(message[0],
                sizeof(Message.MAGIC) + sizeof(Message.COMMAND));

            payload = new byte[payloadSize + 1];

            // Read message type
            // WARNING: not flex
            magic   = (Message.MAGIC)   message[0][0];
            command = (Message.COMMAND) message[0][1];

            // Copy the payload into an array (flex)
            // Safe method would be:
            // i/256 < message.Count && i % 256 < message[(int) i/256].Length
            // But unsafe is a lot faster... just try
            try
            {
                for (UInt32 i = Message.MessageStructureSize, j = 0; j < payloadSize; i++, j++)
                {
                    payload[j] = message[(int) i / 256][i % 256];
                }
                isValid = true;
            }
            catch (Exception e)
            {
                isValid = false;
                Logger.Log("Invalid message received.", Logger.LoggingLevels.MinimalLogging);
                Logger.Debug("Exception: " + e);
            }
        }
    }
}
