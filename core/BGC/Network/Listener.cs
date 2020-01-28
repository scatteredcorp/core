using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BGC.Network
{
    class Listener
    {
        public Queue<byte[]> IncomingQueue;

        private Thread listenerThread;

        private bool keepListening;
        public bool IsRunning => keepListening;

        public Listener()
        {
            IncomingQueue = new Queue<byte[]>();

            listenerThread = new Thread(new ThreadStart(listen));

            keepListening = false;
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
            while (keepListening)
            {
                // Listen
            }
        }
    }
}
