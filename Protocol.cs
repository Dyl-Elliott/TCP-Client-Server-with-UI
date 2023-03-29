using System.Net.Sockets;
using System.Text;

namespace locationserver
{
    class Protocol
    {
        /// <summary>
        /// Whois protocol.
        /// </summary>
        /// <param name="pSocketMessage">message sent in by client.</param>
        /// <param name="socketStream">Network object is used to read and write to and from the server.</param>
        public void WhoisProtocol(string pSocketMessage, NetworkStream socketStream)
        {
            pSocketMessage = pSocketMessage.Replace("\r\n", "");

            if (pSocketMessage.Contains(" ")) // 2 or more arguments passed.
            {
                int spaceIndex = pSocketMessage.IndexOf(" ");

                string name = pSocketMessage.Substring(0, spaceIndex);
                string location = pSocketMessage.Substring(spaceIndex + 1);

                if (Save.Database.ContainsKey(name))
                {
                    Save.Database[name] = location;
                }
                else
                {
                    Save.Database.Add(name, location);
                }

                Output.OutputMessageDecider("Message Returned: OK");
                OutputMessage("OK", socketStream);
            }
            else // 1 argument passed.
            {
                string name = pSocketMessage;

                if (Save.Database.ContainsKey(name))
                {
                    string location = Save.Database[name];
                    Output.OutputMessageDecider("Message Returned: " + location);
                    OutputMessage(location, socketStream);
                }
                else
                {
                    Output.OutputMessageDecider("ERROR: no entries found");
                    OutputMessage("ERROR: no entries found", socketStream);
                }
            }

            socketStream.Close();
        }

        /// <summary>
        /// Https 09 protocol.
        /// Defined by using 'GET /' for a single argument or 'PUT /' for multiple arguments passed.
        /// Additional protocol formatting checks if there are no single line occurrence from arguments.
        /// </summary>
        /// <param name="pSocketMessage">message sent in by client.</param>
        /// <param name="socketStream">Network object is used to read and write to and from the server.</param>
        public void Https09(string pSocketMessage, NetworkStream socketStream)
        {
            if (pSocketMessage.Contains("GET /")) // 1 argument passed.
            {
                pSocketMessage = pSocketMessage.Replace("\r\n", "");
                int indexStartOfName = pSocketMessage.IndexOf('/');
                string name = pSocketMessage.Substring(indexStartOfName + 1);

                if (Save.Database.ContainsKey(name))
                {
                    string location = Save.Database[name];

                    string http09GetMessage = "HTTP/0.9 200 OK\r\n" + "Content-Type: text/plain\r\n\r\n" + location + "\r\n";
                    Output.OutputMessageDecider("Message Returned: " + http09GetMessage);
                    OutputMessage(http09GetMessage, socketStream);
                }
                else
                {
                    string http09GetMessage = "HTTP/0.9 404 Not Found\r\n" + "Content-Type: text/plain\r\n\r\n";
                    Output.OutputMessageDecider("Message Returned: " + http09GetMessage);
                    OutputMessage(http09GetMessage, socketStream);
                }
            }
            else if (pSocketMessage.Contains("PUT /"))
            {
                string name;
                string location;

                // NOT a normal PUT request -->
                if (!pSocketMessage.Contains("\r\n") && pSocketMessage.Contains(" ")) 
                {
                    string[] socketMessage = pSocketMessage.Split(' ');

                    name = socketMessage[0];
                    location = socketMessage[1];
                }
                else
                {
                    string[] socketMessage = FormatSocketMessage(pSocketMessage);

                    int index = socketMessage[0].IndexOf('/');
                    name = socketMessage[0].Substring(index + 1);
                    location = socketMessage[2];
                }

                if (Save.Database.ContainsKey(name))
                {
                    Save.Database[name] = location;
                }
                else
                {
                    Save.Database.Add(name, location);
                }

                string http09PutMessage = "HTTP/0.9 200 OK\r\n" + "Content-Type: text/plain\r\n\r\n";
                Output.OutputMessageDecider("Message Returned: " + http09PutMessage);
                OutputMessage(http09PutMessage, socketStream);
            }

            socketStream.Close();
        }

        /// <summary>
        /// Https 10 protocol.
        /// Defined by using 'GET /' for a single argument or 'POST /' for multiple arguments passed.
        /// </summary>
        /// <param name="pSocketMessage">message sent in by client.</param>
        /// <param name="socketStream">Network object is used to read and write to and from the server.</param>
        public void Https10(string pSocketMessage, NetworkStream socketStream)
        {
            if (pSocketMessage.Contains("GET /"))
            {
                pSocketMessage = pSocketMessage.Replace("\r\n", "");
                int indexStartOfName = pSocketMessage.IndexOf('?');
                int indexEndOfName = pSocketMessage.IndexOf(' ', indexStartOfName);

                string name = pSocketMessage.Substring(indexStartOfName + 1, (indexEndOfName - indexStartOfName) - 1);

                if (Save.Database.ContainsKey(name))
                {
                    string location = Save.Database[name];

                    string http10GetMessage = "HTTP/1.0 200 OK\r\n" + "Content-Type: text/plain\r\n\r\n" + location + "\r\n";
                    Output.OutputMessageDecider("Message Returned: " + http10GetMessage);
                    OutputMessage(http10GetMessage, socketStream);
                }
                else
                {
                    string http10GetMessage = "HTTP/1.0 404 Not Found\r\n" + "Content-Type: text/plain\r\n\r\n";
                    Output.OutputMessageDecider("Message Returned: " + http10GetMessage);
                    OutputMessage(http10GetMessage, socketStream);
                }
            }
            else if (pSocketMessage.Contains("POST /"))
            {
                string[] socketMessage = FormatSocketMessage(pSocketMessage);
                int indexStartOfName = socketMessage[0].IndexOf('/');
                int indexEndOfName = pSocketMessage.IndexOf(' ', indexStartOfName);

                string name = pSocketMessage.Substring(indexStartOfName + 1, (indexEndOfName - indexStartOfName) - 1);
                string location = socketMessage[3];

                if (Save.Database.ContainsKey(name))
                {
                    Save.Database[name] = location;
                }
                else
                {
                    Save.Database.Add(name, location);
                }

                string http10PutMessage = "HTTP/1.0 200 OK\r\n" + "Content-Type: text/plain\r\n\r\n";
                Output.OutputMessageDecider("Message Returned: " + http10PutMessage);
                OutputMessage(http10PutMessage, socketStream);
            }

            socketStream.Close();
        }

        /// <summary>
        /// Https 11 protocol.
        /// Defined by using 'GET /?' for a single argument or 'POST /' for multiple arguments passed.
        /// </summary>
        /// <param name="pSocketMessage">message sent in by client.</param>
        /// <param name="socketStream">Network object is used to read and write to and from the server.</param>
        public void Https11(string pSocketMessage, NetworkStream socketStream)
        {
            if (pSocketMessage.Contains("GET /?"))
            {
                pSocketMessage = pSocketMessage.Replace("\r\n", "");
                int indexStartOfName = pSocketMessage.IndexOf('=');
                int indexEndOfName = pSocketMessage.IndexOf(' ', indexStartOfName);

                string name = pSocketMessage.Substring(indexStartOfName + 1, (indexEndOfName - indexStartOfName) - 1);

                if (Save.Database.ContainsKey(name))
                {
                    string location = Save.Database[name];

                    string http11GetMessage = "HTTP/1.1 200 OK\r\n" + "Content-Type: text/plain\r\n\r\n" + location + "\r\n";
                    Output.OutputMessageDecider("Message Returned: " + http11GetMessage);
                    OutputMessage(http11GetMessage, socketStream);
                }
                else
                {
                    string http11GetMessage = "HTTP/1.1 404 Not Found\r\n" + "Content-Type: text/plain\r\n\r\n";
                    Output.OutputMessageDecider("Message Returned: " + http11GetMessage);
                    OutputMessage(http11GetMessage, socketStream);
                }
            }
            else if (pSocketMessage.Contains("POST /"))
            {
                string[] socketMessage = FormatSocketMessage(pSocketMessage);
                int indexStartOfName = socketMessage[4].IndexOf('=');
                int indexEndOfName = socketMessage[4].IndexOf('&', indexStartOfName);
                int indexStartOfLocation = socketMessage[4].IndexOf('=', indexEndOfName);

                string name = socketMessage[4].Substring(indexStartOfName + 1, (indexEndOfName - indexStartOfName) - 1);
                string location = socketMessage[4].Substring(indexStartOfLocation + 1);

                if (Save.Database.ContainsKey(name))
                {
                    Save.Database[name] = location;
                }
                else
                {
                    Save.Database.Add(name, location);
                }

                string http11PutMessage = "HTTP/1.1 200 OK\r\n" + "Content-Type: text/plain\r\n\r\n";
                Output.OutputMessageDecider("Message Returned: " + http11PutMessage);
                OutputMessage(http11PutMessage, socketStream);
            }

            socketStream.Close();
        }

        /// <summary>
        /// Split client message into individual lines based on a \n to split on.
        /// The proceeding \r is removed from the message to conform to formatting.
        /// </summary>
        /// <param name="pSocketMessage">message sent in by client.</param>
        /// <returns>socket message with \r\n removed from the socketMessage string.</returns>
        private string[] FormatSocketMessage(string pSocketMessage)
        {
            string[] socketMessage = pSocketMessage.Split('\n');
            for (int i = 0; i < socketMessage.Length; i++)
            {
                socketMessage[i] = socketMessage[i].Replace("\r", "");
            }

            return socketMessage;
        }

        /// <summary>
        /// Method used to send message back to the client using a byte array.
        /// </summary>
        /// <param name="pInput">any message which is inserted in this parameter is streamed byte array to be sent
        /// to the client.</param>
        /// <param name="socketStream">Network object is used to read and write to and from the server.</param>
        private static void OutputMessage(string pInput, NetworkStream socketStream)
        {
            byte[] msg = Encoding.ASCII.GetBytes(pInput);
            socketStream.Write(msg, 0, msg.Length); // writes message to client.
            socketStream.Flush();
        }
    }
}
