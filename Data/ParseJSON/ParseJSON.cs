using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParseJSON
{
    public class ParseJSON
    {
        public List<NodeDataModel> DataModels { get; set; }

        public ParseJSON()
        {
            DataModels = new List<NodeDataModel>();
        }

        public void ParseJSONs(ParseDYF dyfObj)
        {
            // DEFINE GLOBALS
            GraphDataParse csvParser = new GraphDataParse();
            NodeDataContainer nodeDataContainer = new NodeDataContainer();
            //GraphDataParseFeaturized csvParserFeaturized = new GraphDataParseFeaturized();

            //Console.WriteLine("Enter Directory Path"); //Prompt user to enter directory to search for PDFS
            //string dirPath = Console.ReadLine(); //Read user input

            string dirPath = "C:\\Users\\pmitev\\Desktop\\graphs";
            string[] fileEntries = Directory.GetFiles(dirPath); //Get all the files of the input directory

            foreach (var filepath in fileEntries)
            {
                Console.WriteLine(filepath);

                using (StreamReader reader = File.OpenText(filepath))
                {
                    // Read JSON file, and get a JToken from it for iterating
                    try
                    {
                        JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                        JToken nodeObject = jObject["Nodes"];
                        JToken connectorObject = jObject["Connectors"];

                        //Define Dictionaries for the node and the In / Out
                        Dictionary<string, string> NodeDictionary = new Dictionary<string, string>();
                        Dictionary<string, string> IODictionary = new Dictionary<string, string>();

                        //Create dictionary for ID and node name
                        foreach (var node in nodeObject)
                        {
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


                                if (dyfObj.customPackageMappings.Keys.Contains(NodeASig))
                                {
                                    var result = dyfObj.customPackageMappings[NodeASig];
                                    NodeASig = result;
                                }

                                if (dyfObj.customPackageMappings.Keys.Contains(NodeBSig))
                                {
                                    var result = dyfObj.customPackageMappings[NodeBSig];
                                    NodeBSig = result;
                                }

                                // CREATE DATA MODEL

                                //if (DataModels.Count < 0)
                                //{
                                //    NodeDataModel newModel = NodeDataModel.CreateNewDataModel(NodeASig, NodeBSig, NodeBID, NodeAID);
                                //    DataModels.Add(newModel);
                                //}

                                if (nodeDataContainer.DataModels.Count > 0)
                                {
                                    
                                    IEnumerable<NodeDataModel> existingRecords = from record in nodeDataContainer.DataModels
                                                                                 where record.NodeAId == NodeAID
                                                                                 select record;

                                    // CHECK IF NODE HAS BEEN ADDED TO RECORD
                                    if (existingRecords.Any())
                                    {
                                        NodeDataModel exisingRecord = existingRecords.First();
                                        exisingRecord.TotalConnectionsCount = +1;

                                        if (exisingRecord.NodeBId != NodeBID)
                                        {
                                            exisingRecord.UniqueConnectionsCount = +1;
                                        }
                                    }
                                    else
                                    {
                                        //NodeDataModel newModel = NodeDataModel.CreateNewDataModel(NodeASig, NodeBSig, NodeBID, NodeAID);
                                        //nodeDataContainer.AppendToDataContainer(newModel);
                                        //DataModels.Add(newModel);
                                    }
                                    
                                }
                                else
                                {
                                    NodeDataModel newModel = NodeDataModel.CreateNewDataModel(NodeASig, NodeBSig, NodeBID, NodeAID);
                                    nodeDataContainer.AppendToDataContainer(newModel);
                                    //DataModels.Add(newModel)
                                    //NodeDataModel newModel = NodeDataModel.CreateNewDataModel(NodeASig, NodeBSig, NodeBID, NodeAID);
                                    //DataModels.Add(newModel);
                                }

                                // CSV DATA DUMP
                                //NodeDataModel newModel = NodeDataModel.CreateNewDataModel(NodeASig, NodeBSig, NodeBID, NodeAID);
                                //DataModels.Add(newModel);
                                //nodeDataContainer.AppendToDataContainer(newModel);
                                NodeDataModel.ParseNodeDataModels(DataModels);

                                csvParser.AppendToCsv(NodeASig, NodeBSig, NodeAID, NodeBID);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("NOT JSON");
                    }

                    csvParser.ExportCSV();
                    var parsetemp = csvParser;
                    var temp = nodeDataContainer.DataModels.Count;
                    Console.WriteLine("EXPORT COMPLETE");
                    //Console.Read();
                }

            }
        }
    }
}
