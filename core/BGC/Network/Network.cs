using System.Net;
using System.Net.Sockets;
using System;

namespace BGC.Network
{
    public class Network
    {
        public enum Codes
        {
            Success = 0,
            SocketException = 1,
            NullException = 2,
            SocketClosedException = 3
        }

        public static Codes SendData(IPEndPoint target, byte[] data)
        {
            Codes r = Codes.Success;
            try
            {
                Logger.Debug("Sending data to " + target);

                Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(target);

                socket.Send(data);
            }
            catch (SocketException e)
            {
                r = Codes.SocketException;
                Logger.Log("SocketException while sending data: " + e + "\nWhile sending data to: " + target, Logger.LoggingLevels.MinimalLogging);
            }
            catch (ObjectDisposedException e)
            {
                r = Codes.SocketClosedException;
                Logger.Log("Socket unexpectedly closed while sending data to " + target, Logger.LoggingLevels.MinimalLogging);
            }
            catch (System.ArgumentNullException)
            {
                r = Codes.NullException;
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