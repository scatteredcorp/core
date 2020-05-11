using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BGC.Consensus;
using Block = BGC.Blockchain.Block;

namespace BGC.Network
{
    public class Network
    {
        public static HashSet<IPEndPoint> nodes = LoadNodes(nodesFile);

        private const string nodesFile = "nodes.txt";
        
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

        public static HashSet<IPEndPoint> LoadNodes(string fileName)
        {
            HashSet<IPEndPoint> nodes = new HashSet<IPEndPoint>();

            foreach (string line in File.ReadLines(fileName))
            {
                try
                {
                    nodes.Add(IPEndPoint.Parse(line));
                }
                catch {}
            }
            if(nodes == null) nodes = new HashSet<IPEndPoint>();
            return nodes;
        }

        public static void SaveNodes(HashSet<IPEndPoint> nodes, string fileName)
        {
            string result = "";

            foreach (IPEndPoint node in nodes)
            {
                result += node + "\n";
            }
            
            File.WriteAllText(fileName, result);
        }

        public static void AddNode(IPEndPoint node)
        {
            if (nodes.Contains(node))
                return;
            nodes.Add(node);
            SaveNodes(nodes, nodesFile);
        }

        public static void RegisterNode(byte[] payload)
        {
            AddNode(IPEndPoint.Parse(Encoding.ASCII.GetString(payload)));
        }

        public static void PropagateBlock(Block block)
        {
            byte[] message = Utils.CreateMessage(Message.COMMAND.SendBlock, block.Serialize());
            ReturnCode WeDontCare = ReturnCode.Pending;
            foreach (IPEndPoint node in nodes)
            {
                SendData(node, message, ref WeDontCare);
            }
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
