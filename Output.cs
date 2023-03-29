using System;

namespace locationserver
{
    class Output
    {
        private static bool _printToConsole = true;
        private static locationserverUI _locationServerUi;

        /// <summary>
        /// Sets the _locationServerUi field to an instance of the LocationServerUI class 
        /// in order to make it accessible for outputting information to the user via the list box.
        /// </summary>
        /// <param name="pLocationUi">recieves UI object to be set in this class.</param>
        public static void SetLocationUi(locationserverUI pLocationUi)
        {
            _locationServerUi = pLocationUi;
        }

        /// <summary>
        /// Message check to see whether _printToConsole has been changes to false (default = true).
        /// Doing this allows the program to decide if to print the messages to the console
        /// or to the UI window.
        /// </summary>
        /// <param name="pMessage"></param>
        public static void OutputMessageDecider(string pMessage)
        {
            if (_printToConsole)
            {
                OutputToConsole(pMessage);
            }
            else
            {
                OutputToInterface(pMessage);
            }
        }

        /// <summary>
        /// Sets whether to print to Console Window or not depending on if UI interface is launched.
        /// </summary>
        /// <param name="pResult">passed a true or false result depending on pre logic.</param>
        public static void PrintToConsoleSet(bool pResult)
        {
            _printToConsole = pResult;
        }

        /// <summary>
        /// Prints to console if _printToConsole would be set to true.
        /// </summary>
        /// <param name="pMessage"></param>
        public static void OutputToConsole(string pMessage)
        {
            Console.WriteLine(pMessage);
        }

        /// <summary>
        /// Operation that will be executed on the UI thread.
        /// </summary>
        /// <param name="pMessage">message to be passed to list viewer.</param>
        public static void OutputToInterface(string pMessage)
        {
            _locationServerUi.Dispatcher.Invoke(() =>
            {
                _locationServerUi.nListViewer.Items.Add(pMessage);
            });
        }
    }
}