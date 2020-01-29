using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;

namespace BGC.Network
{
    public class Network
    {
        public enum ReturnCode
        {
            Success = 0,
            SocketException = 1,
            NullException = 2,
            SocketClosedException = 3
        }

        public static async Task<ReturnCode> SendData(IPEndPoint target, byte[] data)
        {
            ReturnCode r = ReturnCode.Success;
            try
            {
                Logger.Debug("Sending data to " + target);

                Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(target);

                socket.Send(data);
            }
            catch (SocketException e)
            {
                r = ReturnCode.SocketException;
                Logger.Log("SocketException while sending data: " + e + "\nWhile sending data to: " + target, Logger.LoggingLevels.MinimalLogging);
            }
            catch (ObjectDisposedException e)
            {
                r = ReturnCode.SocketClosedException;
                Logger.Log("Socket unexpectedly closed while sending data to " + target, Logger.LoggingLevels.MinimalLogging);
            }
            catch (System.ArgumentNullException)
            {
                r = ReturnCode.NullException;
                Logger.Log("NullException while sending data", Logger.LoggingLevels.HighLogging);
            }
            finally
            {
                Logger.Debug("Successfully sent data to " + target);
            }

            return r;
        }
    }
}