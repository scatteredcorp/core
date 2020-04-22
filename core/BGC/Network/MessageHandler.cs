using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BGC.Network {
    public static class MessageHandler {
        public static void Handle(NetworkMessage message, ref bool requestExit) {
            Logger.Debug($"Handling message of type {message.command} on network {message.magic}");
            switch (message.command)
            {
                case Message.COMMAND.GetBlock:
                    break;
                case Message.COMMAND.SendBlock:
                    Logger.Debug($"Received block {(global::BGC.Utils.ToHex(Blockchain.Blockchain.DeserializeBlock(message.Payload).BlockHeader.Hash()))}");
                    break;
                case Message.COMMAND.GetVersion:
                    break;
                case Message.COMMAND.SendVersion:
                    break;
                case Message.COMMAND.GetHeaders:
                    break;
                case Message.COMMAND.SendHeaders:
                    break;
                case Message.COMMAND.GetMempool:
                    break;
                case Message.COMMAND.SendMempool:
                    break;
                case Message.COMMAND.GetContracts:
                    break;
                case Message.COMMAND.TextMessage:
                    Logger.Debug($"Text: {Encoding.ASCII.GetString(message.Payload)}");
                    break;
                case Message.COMMAND.SendContract:
                    break;
                case Message.COMMAND.GetInventory:
                    break;
                case Message.COMMAND.SendInventory:
                    break;
                case Message.COMMAND.GetPublicKey:
                    break;
                case Message.COMMAND.SendPublicKey:
                    break;
                case Message.COMMAND.RegisterNode:
                    Network.RegisterNode(message.Payload);
                    break;
                default:
                    break;
            }
        }
    }
}