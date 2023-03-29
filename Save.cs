using System;
using System.Collections.Generic;
using System.IO;

namespace locationserver
{
    public class Save
    {
        public static Dictionary<string, string> Database = new Dictionary<string, string>();
        private static string _fileName;

        public Save()
        { }

        public Save(string pDataState)
        {
            _fileName = pDataState;
        }

        /// <summary>
        /// Used to save information stored to file if exists in database.
        /// </summary>
        public static void SaveFile()
        {
            string inputToSave = null;
            if (Database.Count >= 1)
            {
                foreach (var line in Database)
                {
                    inputToSave = line.ToString();
                }

                if (Database == null)
                {
                    return;
                }
                else
                {
                    try
                    {
                        StreamWriter w;
                        using (w = File.AppendText(_fileName))
                        {
                            w.WriteLine(inputToSave);
                        }

                        w.Close();
                    }

                    catch (Exception e)
                    {
                        Output.OutputMessageDecider("Unable to write save file" + e);
                    }
                }
            }
        }

        /// <summary>
        /// Used to attempt to load a previous saved file from client message is previously ran.
        /// A new list is used to remove '[' ']' and ',' from saved message to allow
        /// output of name and location if found in database without any formatting errors.
        /// </summary>
        public static void LoadFile()
        {
            if (File.Exists(_fileName))
            {
                List<string> saveData = new List<string>();

                using (StreamReader reader = new StreamReader(_fileName))
                {
                    string saveDataLine;
                    while ((saveDataLine = reader.ReadLine()) != null)
                    {
                        saveData.Add(saveDataLine);
                    }
                }

                for (int i = 0; i < saveData.Count; i++)
                {
                    saveData[i] = saveData[i].Replace("[", "");
                    saveData[i] = saveData[i].Replace("]", "");
                    saveData[i] = saveData[i].Replace(",", "");
                }

                foreach (var line in saveData)
                {
                    string name = null;
                    string location = null;
                    if (line != null)
                    {
                        int spaceIndex = line.IndexOf(" ");
                        name = line.Substring(0, spaceIndex);
                        location = line.Substring(spaceIndex + 1);
                    }

                    if (name != null && Database.ContainsKey(name))
                    {
                        Database[name] = location;
                    }
                    else
                    {
                        Database.Add(name, location);
                    }
                }
            }
            else
            {
                Output.OutputMessageDecider("A file to load from could not be found...");
                Output.OutputMessageDecider("The Database does not contain any records.");
            }
        }
    }
}
