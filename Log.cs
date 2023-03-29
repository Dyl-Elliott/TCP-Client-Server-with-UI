using System;
using System.IO;

namespace locationserver
{
    public class Log
    {
        private static readonly object Locker = new object();
        public static string LogFile = null;

        public Log()
        { }

        public Log(string pFileName)
        {
            LogFile = pFileName;
        }

        /// <summary>
        /// Imports link message to log file if flag -l has been specified in arguments.
        /// </summary>
        /// <param name="pHostIp">ip address from which the client message has been received from.</param>
        /// <param name="pMessage">actual message received from the client.</param>
        /// <param name="pStatus">certifies submission was OK.</param>
        public static void WriteToLog(string pHostIp, string pMessage, string pStatus)
        {
            string linker = pHostIp + " - - " + DateTime.Now.ToString("'['dd/MM/yyyy' : 'HH:mm' : 'ss zz00']'") + " \"" + pMessage.Trim() + "\" " + pStatus;

            lock (Locker) // manages to 1 write at a time-->
            {
                if (LogFile == null)
                {
                    return;
                }
                else
                {
                    try
                    {
                        StreamWriter w;
                        using (w = File.AppendText(LogFile))
                        {
                            w.WriteLine(linker);
                        }

                        w.Close();
                    }

                    catch (Exception)
                    {
                        Output.OutputMessageDecider("Unable to write log file" + LogFile);
                    }
                }
            }
        }

        /// <summary>
        /// Method used to check log and save file information if provided in args either via UI
        /// or console window.
        /// </summary>
        /// <param name="args">arguments passed from client command window or UI.</param>
        public static void LogAndSaveFileFlagCheck(string[] args)
        {
            int maxIndex = args.Length - 1; // prevents out of bounds
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-l")
                {
                    if (i < maxIndex)
                    {
                        string filename = args[i + 1];
                        Program.Logger = new Log(filename);
                    }
                }
                else if (args[i] == "-f")
                {
                    if (i < maxIndex) // stop index error
                    {
                        string dataState = args[i + 1];
                        Program.DatabaseLogger = new Save(dataState);
                    }

                    Save.LoadFile();
                }
            }
        }

        /// <summary>
        /// If either (or both) a -l or -f flag have been provided then
        /// subsequent methods are ran to save data into these files.
        /// </summary>
        /// <param name="args">arguments passed from client command window or UI.</param>
        /// <param name="pHostIp">passed in ip address.</param>
        /// <param name="socketMessage">passed in client message.</param>
        /// <param name="pStatus">OK message.</param>
        public static void LogAndSaveFileWrite(string[] args, string pHostIp, string socketMessage, string pStatus)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-l")
                {
                    Log.WriteToLog(pHostIp, socketMessage, pStatus); // save to a log file.
                }
                if (args[i] == "-f")
                {
                    Save.SaveFile(); // state save of database.
                }
            }
        }
    }
}