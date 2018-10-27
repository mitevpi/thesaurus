using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParseJSON;

namespace thesaurus
{
    class ParseDYN
    {

        public static void ParseDynData(string dirPath)
        {
            // DEFINE GLOBALS
            //string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string filePath = dirPath + "\\MPA_Title Block_Key Plan Control.dyn";
            DataParse csvParser = new DataParse();
            string[] fileEntries = Directory.GetFiles(dirPath); //Get all the files of the input directory

            foreach (string fileName in fileEntries) //Loop over all the files in teh directory
            {
                string dynTitle = System.IO.Path.GetFileName(fileName);
                dynTitle = dynTitle.Remove(dynTitle.Length - 4);
                Console.WriteLine(dynTitle);

                using (StreamReader reader = File.OpenText(fileName))
                {
                    // Read JSON file, and get a JToken from it for iterating
                    JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                    //Define the Nodes into a JToken
                    JToken nodeObject = jObject["Nodes"];
                    JToken connectorObject = jObject["Connectors"];

                    //Define Dictionaries for the node and the In / Out
                    Dictionary<string, string> NodeDictionary = new Dictionary<string, string>();
                    Dictionary<string, string> IODictionary = new Dictionary<string, string>();

                    //int countfound;
                    //int countnotfound;

                    //Console.Write(nodeObject);

                    //Create dictionary for ID and node name
                    foreach (var node in nodeObject)
                    {
                        //Console.WriteLine(test);
                        //Console.WriteLine(node["FunctionSignature"]);
                        string stringnodeID = node["Id"].ToString();

                        string stringnodename = "NONE";

                        try
                        {
                            stringnodename = node["FunctionSignature"].ToString();
                            Console.WriteLine(stringnodename);
                        }
                        catch
                        {
                            Console.WriteLine("MISSING FUNCTION SIGNATURE");
                        }

                        if (stringnodename != "NONE")
                        {
                            NodeDictionary.Add(stringnodeID, stringnodename);


                            JToken outputObject = node["Outputs"];

                            foreach (var output in outputObject)
                            {
                                string outputID = output["Id"].ToString();
                                IODictionary.Add(outputID, stringnodeID);
                            }
                            JToken inputObject = node["Inputs"];

                            foreach (var inputs in inputObject)
                            {
                                string inputID = inputs["Id"].ToString();
                                IODictionary.Add(inputID, stringnodeID);
                            }
                            //Console.WriteLine(IODictionary);
                        }

                    }


                    foreach (var connector in connectorObject)
                    {
                        string inputID = connector["Start"].ToString();
                        string outputID = connector["End"].ToString();

                        try
                        {
                            string NodeAID = IODictionary[inputID];
                            string NodeBID = IODictionary[outputID];

                            string NodeASig = NodeDictionary[NodeAID];
                            string NodeBSig = NodeDictionary[NodeBID];

                            Console.WriteLine(NodeAID);
                            Console.WriteLine(NodeBID);
                            Console.WriteLine(NodeASig);
                            Console.WriteLine(NodeBSig);

                            csvParser.AppendToCsv(NodeASig, NodeBSig, NodeAID, NodeBID);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            csvParser.ExportCSV();
            Console.Read();

        }
    }
}
