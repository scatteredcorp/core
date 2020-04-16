using System.Collections.Generic;
using System.Net;

namespace BGC.Network {
    public static class MessageHandler {
        public static void Handle(NetworkMessage message) {
            switch (message.command)
            {
                case Message.COMMAND.GetBlock:
                    break;
                case Message.COMMAND.SendBlock:
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
                    break;
                case Message.COMMAND.SendContract:
                    break;
                default:
                    break;
            }
        }
    }
}