using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using BGC.Network;
using BGC.Wallet;
using CommandLine;

namespace BGC.CLI {
	public static class NetworkCmd {
		[Verb("start", HelpText = "Start network")]
		public class StartOptions {
			[Option(Group = "start", HelpText="Port")]
			public int Port { get; set; }
			[Option(Group = "start", HelpText="Max nodes")]
			public int Max { get; set; }
		}

		public static void Start(StartOptions opts) {
			if (opts.Port == 0) {
				throw new Exception("Port required");
			}

			if (opts.Max == 0) {
				throw new Exception("Max nodes required");
			}
			
			Blockchain.Blockchain.Resume();
			Callback.Callback.Run(opts.Port, opts.Max);
		}

		[Verb("send", HelpText = "Send data")]
		public class SendDataOptions {
			[Value(0)]
			public string Data { get; set; }
			
			[Option(Group = "send", HelpText="IP")]
			public string IP { get; set; }
			[Option(Group = "send", HelpText="Port")]
			public int Port { get; set; }
		}
		public static void SendData(SendDataOptions opts) {
			if (string.IsNullOrEmpty(opts.Data)) {
				throw new Exception("Message required");
			}
			
			IPAddress localAddress = IPAddress.Parse(opts.IP);
			IPEndPoint ip = new IPEndPoint(localAddress, opts.Port);
            
            byte[] msg = Network.Utils.CreateTextMessage(opts.Data);
			Network.Network.ReturnCode code = Network.Network.ReturnCode.Pending;
			Network.Network.SendData(ip, msg, ref code);
			Thread.Sleep(5000);
		}
		
	}
}
