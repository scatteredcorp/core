using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BGC.Network
{
    class Listener
    {
        public Queue<NetworkMessage> IncomingQueue;

        private Thread listenerThread;

        private bool keepListening;
        public bool IsRunning => keepListening;

        private int port;
        public int Port => port;

        private int maxClientsQueue;
        public int MaxClientsQueue => maxClientsQueue;

        private int updateInterval;

        public Listener(int port, int maxClientsQueue, int serverUpdateInterval = 50)
        {
            IncomingQueue = new Queue<NetworkMessage>();

            listenerThread = new Thread(new ThreadStart(listen));

            keepListening = false;

            this.port = port;
            this.maxClientsQueue = maxClientsQueue;
            updateInterval = serverUpdateInterval;
        }

        public void StartListening()
        {
            keepListening = true;

            listenerThread.Start();

            Logger.Log("Listener thread started.", Logger.LoggingLevels.Debug);
        }

        public void StopListening()
        {
            keepListening = false;

            Logger.Log("Sent request stop request to the listener.", Logger.LoggingLevels.Debug);
        }

        private void listen()
        {
            TcpListener tcpServer = null;

            try
            {
                // Set the local address
                IPAddress localAddress = IPAddress.Parse("0.0.0.0");

                // Init the server
                tcpServer = new TcpListener(localAddress, port);
                
                // Start listening to client requests
                tcpServer.Start(maxClientsQueue);

                Logger.Log("Successfully started the TCP server on port " + port, Logger.LoggingLevels.HighLogging);

                // Buffer for incoming data
                byte[] buffer;

                while (keepListening)
                {
                    Logger.Log("Waiting for an incoming connection...", Logger.LoggingLevels.HighLogging);

                    while (keepListening && !tcpServer.Pending())
                    {
                        Thread.Sleep(updateInterval);
                    }

                    if (!keepListening)
                        break;

                    TcpClient tcpClient = tcpServer.AcceptTcpClient();

                    Socket socket = tcpClient.Client;

                    Logger.Log("Connected to a TCP client !", Logger.LoggingLevels.HighLogging);
                    Logger.Debug("Connected to " + socket.RemoteEndPoint.ToString());

                    buffer = new byte[256];

                    // Read first packet to know the expected size

                    List<byte[]> payload = new List<byte[]>();
                    int recv = socket.Receive(buffer);

                    UInt32 expectedSize = BitConverter.ToUInt32(buffer, sizeof(Message.MAGIC) + sizeof(Message.COMMAND)) + Message.MessageStructureSize;

                    payload.Add(buffer);

                    Logger.Log("Received header bytes: " + Encoding.Unicode.GetString(buffer, (int) Message.MessageStructureSize, recv), Logger.LoggingLevels.HighLogging);

                    while (recv < expectedSize)
                    {
                        recv = socket.Receive(buffer);

                        Logger.Log("Received bytes: " + Encoding.ASCII.GetString(buffer, 0, recv), Logger.LoggingLevels.HighLogging);

                        payload.Add(buffer);
                    }
                    IncomingQueue.Enqueue(new NetworkMessage(payload, socket.RemoteEndPoint as IPEndPoint));
                    tcpClient.Close();

                    Logger.Log("Done receiving data; connection closed.", Logger.LoggingLevels.HighLogging);
                }

                Logger.Log("Listener: received stop request; stopping...", Logger.LoggingLevels.HighLogging);
            }
            catch (SocketException e) {
                Logger.Log("SocketException while listening: " + e, Logger.LoggingLevels.MinimalLogging);
            }
            catch (IOException e) {
                Logger.Log("IOException while listening: " + e, Logger.LoggingLevels.MinimalLogging);
            }
            finally {
                // Register the listener as stopped, so it doesn't get stuck in case of errors
                keepListening = false;

                tcpServer.Stop();

                Logger.Log("Stopped the TCP server", Logger.LoggingLevels.HighLogging);
            }
        }
    }
}
