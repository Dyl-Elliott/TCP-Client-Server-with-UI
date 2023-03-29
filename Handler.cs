using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace locationserver
{
    public class Handler
    {
        public static bool Debugger { get; set; }
        private static int _timeout;
        private static bool _timeoutOnce;

        /// <summary>
        /// Main method to run request sent by the client to this server.
        /// Details and holds received message from client, including setting any
        /// timeout, debugging and saving of log and save file requirements.
        /// </summary>
        /// <param name="pConnection">allows Socket operations to be performed to provide access to stream data from client.</param>
        /// <param name="args">arguments passed from client command window or UI.</param>
        /// <returns></returns>
        public static string DoRequest(Socket pConnection, string[] args)
        {
            string hostIp = ((IPEndPoint)pConnection.RemoteEndPoint).Address.ToString();
            string status = "OK";
            Debugger = CheckDebugFlag(args);
            
            if (Debugger)
            {
                Output.OutputMessageDecider("Debugging has been activated.");
            }

            pConnection.ReceiveTimeout = CheckTimeoutFlag(args);
            pConnection.SendTimeout = CheckTimeoutFlag(args);

            if (_timeoutOnce == false)
            {
                Output.OutputMessageDecider("Timeout has been set to: " + _timeout);
                _timeoutOnce = true;
            }

            NetworkStream socketStream = new NetworkStream(pConnection);
            byte[] buffer = new byte[256]; 
            int bytes = socketStream.Read(buffer, 0, buffer.Length); 
            string socketMessage = Encoding.ASCII.GetString(buffer, 0, bytes); 

            if (Debugger)
            {
                Output.OutputMessageDecider("Client message received: " + socketMessage);
            }

            ProtocolCheck(socketMessage, socketStream);
            Log.LogAndSaveFileWrite(args, hostIp, socketMessage, status);

            pConnection.Close();
            socketStream.Close();

            return socketMessage;
        }

        /// <summary>
        /// Checks if debugging flag has been found from either UI or console.
        /// </summary>
        /// <param name="args">arguments passed from client command window or UI.</param>
        /// <returns>true or false depending on flag discovery check.</returns>
        public static bool CheckDebugFlag(string[] args)
        {
            foreach (var line in args)
            {
                if (line == "-d")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// If a timeout flag -t has been specified either sets timeout to 0 if not then
        /// provided timeout number, if a timeout number has been specified to the index after the
        /// flag then is set to this.
        /// </summary>
        /// <param name="args">arguments passed from client command window or UI.</param>
        /// <returns>value of set timeout.</returns>
        public static int CheckTimeoutFlag(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-t")
                {
                    
                    if (args[i] + 1 == "")
                    {
                        _timeout = 0;
                    }
                    else
                    {
                        _timeout = int.Parse(args[i + 1]);
                    }

                    
                    return _timeout;
                }
            }

            return 1000; // default
        }

        /// <summary>
        /// Chooses which protocol to run depending on which flag has been set and sent
        /// from the client.
        /// Additional checks are made to make sure socket message are conforming to the correct protocol formatting.
        /// </summary>
        /// <param name="socketMessage">pulled transaction data from network connection provided by the client.</param>
        /// <param name="socketStream">used to connect with Socket to allow data to be passed through to protocols.</param>
        public static void ProtocolCheck(string socketMessage, NetworkStream socketStream)
        {
            Protocol p = new Protocol();

            string[] str = socketMessage.Split('\n');
            if (socketMessage.Contains("HTTP/1.1") && socketMessage.Contains("Host:") && (socketMessage.Contains("GET /?name=") || socketMessage.Contains("POST / ")))
            {
                p.Https11(socketMessage, socketStream);
            }
            else if (socketMessage.Contains("HTTP/1.0") && (socketMessage.Contains("GET /?") || socketMessage.Contains("POST /")))
            {
                p.Https10(socketMessage, socketStream);
            }
            else if (socketMessage.Contains("GET /") || socketMessage.Contains("PUT /") && str.Length > 2)
            {
                p.Https09(socketMessage, socketStream);
            }
            else
            {
                p.WhoisProtocol(socketMessage, socketStream);
            }
        }
    }
}
