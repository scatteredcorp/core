﻿using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace BGC.Network
{
    public class Network
    {
        public enum ReturnCode
        {
            Success = 0,
            SocketException = 1,
            NullException = 2,
            SocketClosedException = 3,
            InvalidMessageSizeException = 4,
            Pending = -1
        }

        private class Parameters
        {
            public IPEndPoint target;
            public byte[] data;
            public ReturnCode returnCode;

            public Parameters(IPEndPoint t, byte[] d, ref ReturnCode rc)
            {
                target = t;
                data = d;
                returnCode = rc;
            }
        }

        public static void SendData(IPEndPoint target, byte[] data, ref ReturnCode returnCode)
        {
            if (!IsMsgComplete(data)) {
                returnCode = ReturnCode.InvalidMessageSizeException;
                Console.WriteLine("Message incomplete");
                //return;
            }

            Parameters p = new Parameters(target, data, ref returnCode);

            Thread thread = new Thread(new ParameterizedThreadStart(SendDataSync));

            thread.Start(p);
        }

        private static void SendDataSync(object o)
        {
            Parameters p = o as Parameters;
            try
            {
                Logger.Debug("Sending data to " + p.target);

                Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(p.target);

                socket.Send(p.data);

                Thread.Sleep(2000);

                p.returnCode = ReturnCode.Success;

                Logger.Debug("Successfully sent data to " + p.target);
            }
            catch (SocketException e)
            {
                p.returnCode = ReturnCode.SocketException;
                Logger.Log("SocketException while sending data: " + e + "\nWhile sending data to: " + p.target, Logger.LoggingLevels.MinimalLogging);
            }
            catch (ObjectDisposedException e)
            {
                p.returnCode = ReturnCode.SocketClosedException;
                Logger.Log("Socket unexpectedly closed while sending data to " + p.target, Logger.LoggingLevels.MinimalLogging);
                Logger.Debug("ObjectDisposedException: " + e);
            }
            catch (System.ArgumentNullException)
            {
                p.returnCode = ReturnCode.NullException;
                Logger.Log("NullException while sending data", Logger.LoggingLevels.HighLogging);
            }
            finally
            {
                
            }
        }

        private static bool IsMsgComplete(byte[] message)
        {
            return BitConverter.ToUInt32(message, sizeof(Message.MAGIC) + sizeof(Message.COMMAND)) // Payload size
                + Message.MessageStructureSize  // Minimal structure size
                == message.Length;              // Total size
        }
    }
}
