using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

// -l "C:\Log\logfile.txt" -f "savefile.txt
// latest version with UI on git with debugging 
namespace locationserver
{
    class Program
    {
        public static Log Logger;
        public static Save DatabaseLogger;
        public static string LogFile { get; set; }
        public static string SaveFile { get; set; }

        [STAThread]
        private static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Log.LogAndSaveFileFlagCheck(args);

                if (args[i] == "-w") //run UI
                {
                    Output.PrintToConsoleSet(false);

                    locationserverUI ui = new locationserverUI(LogFile, SaveFile); // passes log files from command line (if passed) to act as a set values.
                    Window w = new Window
                    {
                        Content = ui,
                        Height = 480,
                        Width = 720
                    };

                    w.ShowDialog();
                }
            }

            // runs if UI does not with no -w flag discovered -->
            RunServer(args);
        }

        /// <summary>
        /// Main server running method.
        /// Initializes TCP, Socket and Threading doRequest(for console window run).
        /// </summary>
        /// <param name="args">arguments to be passed in.</param>
        public static void RunServer(string[] args)
        {
            Log.LogAndSaveFileFlagCheck(args);

            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 43); // listens for incoming connection requests
                listener.Start(); // queue connections
                Output.OutputMessageDecider("Server is listening. . . . .");

                while (true)
                {
                    // Returns a socket object for network communication
                    Socket connection = listener.AcceptSocket(); // forms the connection to the TCP listener. Pulls request from queue

                    if (Handler.Debugger)
                    {
                        Output.OutputMessageDecider("connection Established... \nConnection point: " + connection.RemoteEndPoint);
                    }

                    Socket connectionEstablished = connection;
                    Thread t = new Thread(() => Handler.DoRequest(connectionEstablished, args));
                    t.Start();
                    //Thread.Sleep(300);
                }
            }
            catch (Exception e)
            {
                Output.OutputMessageDecider("Exception captured. Error: " + e + " occurred");
            }
        }
    }
}
