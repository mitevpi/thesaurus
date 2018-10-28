using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParseJSON;

namespace thesaurus
{
    class ParseDYN
    {

        public static List<string[]> ParseDynData(List<string> filePaths)
        {
            // DEFINE GLOBALS
            DataParse csvParser = new DataParse();
            List<string[]> trainingData = new List<string[]>();

            foreach (var fileName in filePaths) //Loop over all the files in teh directory
            {
                string dynTitle = System.IO.Path.GetFileName(fileName);
                dynTitle = dynTitle.Remove(dynTitle.Length - 4);
                Console.WriteLine(dynTitle);

                using (StreamReader reader = File.OpenText(fileName))
                {
                    try
                    {
                        // Read JSON file, and get a JToken from it for iterating
                        JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                        //Define the Nodes into a JToken
                        JToken nodeObject = jObject["Nodes"];
                        JToken connectorObject = jObject["Connectors"];

                        //Define Dictionaries for the node and the In / Out
                        Dictionary<string, string> NodeDictionary = new Dictionary<string, string>();
                        Dictionary<string, string> IODictionary = new Dictionary<string, string>();

                        //Console.Write(nodeObject);

                        //Create dictionary for ID and node name
                        foreach (var node in nodeObject)
                        {
                            string stringnodeID = node["Id"].ToString();

                            string stringnodename = string.Empty;

                            try
                            {
                                stringnodename = node["FunctionSignature"].ToString();
                                Console.WriteLine(stringnodename);
                            }
                            catch
                            {
                                Console.WriteLine("MISSING FUNCTION SIGNATURE");
                                stringnodename = node["ConcreteType"].ToString().Split(',')[0];
                            }

                            if (!string.IsNullOrEmpty(stringnodename))
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

                                string[] dataObservation = new string[] { NodeASig, NodeBSig };
                                trainingData.Add(dataObservation);

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
                    catch
                    {

                    }
                }
            }
            csvParser.ExportCSV();
            Console.Read();

            return trainingData;
        }
    }
}
