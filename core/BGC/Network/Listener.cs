using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace BGC.Network
{
    class Listener
    {
        public Queue<(byte[], IPAddress)> IncomingQueue;

        private Thread listenerThread;

        private bool keepListening;
        public bool IsRunning => keepListening;

        private int port;
        public int Port => port;

        private int maxClientsQueue;
        public int MaxClientsQueue => maxClientsQueue;

        public Listener(int port, int maxClientsQueue)
        {
            IncomingQueue = new Queue<(byte[], IPAddress)>();

            listenerThread = new Thread(new ThreadStart(listen));

            keepListening = false;

            this.port = port;
            this.maxClientsQueue = maxClientsQueue;
        }

        public void StartListening()
        {
            keepListening = true;

            listenerThread.Start();
        }

        public void StopListening()
        {
            keepListening = false;
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

                // Buffer for incoming data
                byte[] buffer = new byte[256];
                string data = null;

                while (keepListening)
                {

                }
            }
            catch { }
            finally {
                // Register the listener as stopped, so it doesn't get stuck in case of errors
                keepListening = false;
            }
        }
    }
}
