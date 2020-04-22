using System.Text;
using System.Threading;
using BGC.Network;

namespace BGC.Callback
{
    public class Callback
    {
        public static bool exitRequested = false;
        
        public static void Run(int port, int maxClients)
        {
            Listener listener = new Listener(port, maxClients);
            
            listener.StartListening();

            while (!exitRequested)
            {
                listener.QueueMutex.WaitOne();
                if (listener.IncomingQueue.Count == 0)
                {
                    listener.QueueMutex.ReleaseMutex();
                    Thread.Sleep(50);
                    continue;
                }

                NetworkMessage msg = listener.IncomingQueue.Dequeue();
                listener.QueueMutex.ReleaseMutex();
                
                MessageHandler.Handle(msg, ref exitRequested);
            }
        }
    }
}