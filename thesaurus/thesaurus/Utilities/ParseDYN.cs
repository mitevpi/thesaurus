using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParseJSON;

namespace thesaurus
{
    public class ParseDYN
    {
        public static List<string[]> ParseDynData(List<string> filePaths)
        {
            // (Petr) DEFINE GLOBALS
            var csvParser = new DataParse();
            var trainingData = new List<string[]>();

            foreach (var fileName in filePaths) //Loop over all the files in the directory
            {
                var dynTitle = Path.GetFileName(fileName);
                dynTitle = dynTitle.Remove(dynTitle.Length - 4);
                Console.WriteLine(dynTitle);

                using (var reader = File.OpenText(fileName))
                {
                    try
                    {
                        // (Petr) Read JSON file, and get a JToken from it for iterating
                        var jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                        // (Petr) Define the Nodes into a JToken
                        var nodeObject = jObject["Nodes"];
                        var connectorObject = jObject["Connectors"];

                        // (Petr) Define Dictionaries for the node and the In / Out
                        var NodeDictionary = new Dictionary<string, string>();
                        var IODictionary = new Dictionary<string, string>();

                        // (Petr) Create dictionary for ID and node name
                        foreach (var node in nodeObject)
                        {
                            var stringnodeID = node["Id"].ToString();

                            string stringnodename;

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
                                var outputObject = node["Outputs"];

                                foreach (var output in outputObject)
                                {
                                    var outputID = output["Id"].ToString();
                                    IODictionary.Add(outputID, stringnodeID);
                                }
                                var inputObject = node["Inputs"];

                                foreach (var inputs in inputObject)
                                {
                                    var inputID = inputs["Id"].ToString();
                                    IODictionary.Add(inputID, stringnodeID);
                                }
                            }

                        }

                        foreach (var connector in connectorObject)
                        {
                            var inputID = connector["Start"].ToString();
                            var outputID = connector["End"].ToString();

                            try
                            {
                                var NodeAID = IODictionary[inputID];
                                var NodeBID = IODictionary[outputID];

                                var NodeASig = NodeDictionary[NodeAID];
                                var NodeBSig = NodeDictionary[NodeBID];

                                var dataObservation = new[] { NodeASig, NodeBSig };
                                trainingData.Add(dataObservation);

                                Console.WriteLine(NodeAID);
                                Console.WriteLine(NodeBID);
                                Console.WriteLine(NodeASig);
                                Console.WriteLine(NodeBSig);

                                csvParser.AppendToCsv(NodeASig, NodeBSig, NodeAID, NodeBID);
                            }
                            catch (Exception)
                            {
                                //ignored
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //ignored
                    }
                }
            }
            csvParser.ExportCSV();
            Console.Read();

            return trainingData;
        }
    }
}
