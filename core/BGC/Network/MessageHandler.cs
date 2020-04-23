using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BGC.Consensus;
using BGC.Network;
using Block = BGC.Blockchain.Block;

namespace BGC.Network {
    public static class MessageHandler {
        private static Network.ReturnCode dummy;
        public static void Handle(NetworkMessage message, ref bool requestExit) {
            Logger.Debug($"Handling message of type {message.command} on network {message.magic}");
            switch (message.command)
            {
                case Message.COMMAND.GetBlock:
                    break;
                case Message.COMMAND.SendBlock:
                    Block block = Blockchain.Blockchain.DeserializeBlock(message.Payload);
                    Logger.Debug($"Received block {(global::BGC.Utils.ToHex(block.BlockHeader.Hash()))}");
                    Blockchain.Blockchain.PushBlock(block);
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
                case Message.COMMAND.UnityGetInventory:
                    Logger.Debug("Unity requested inventory. Sending back empty message.");
                    Network.SendData(IPEndPoint.Parse("127.0.0.1:27496"), Utils.CreateMessage(Message.COMMAND.UnitySendInventory, new byte[1], Message.MAGIC.LocalNetwork), ref dummy);
                    break;
                default:
                    break;
            }
        }
    }
}