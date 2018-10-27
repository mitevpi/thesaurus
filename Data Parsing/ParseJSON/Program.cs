using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParseJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseJSON();
            Console.ReadKey();

        }

        public static void ParseJSON()
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = dirPath + "\\Test Graph_PP_02.dyn";

            using (StreamReader reader = File.OpenText(filePath))
            {
                // Read JSON file, and get a JToken from it for iterating
                JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                JToken nodeObject = jObject["Nodes"];
<<<<<<< HEAD
                JToken connectorObject = jObject["Connectors"];

                Dictionary<string, string> NodeDictionary =new Dictionary<string, string>();
                Dictionary<string, string> IODictionary = new Dictionary<string, string>();

                //Console.Write(nodeObject);

                //Create dictionary for ID and node name
                //Create dictionary for connector ID and node ID
                foreach (var node in nodeObject)
                {
                    //Console.WriteLine(test);
                    Console.WriteLine(node["FunctionSignature"]);
                    string stringnodeID = node["Id"].ToString();
                    string stringnodename = node["FunctionSignature"].ToString();

                    NodeDictionary.Add(stringnodeID, stringnodename);
             

                    JToken outputObject = nodeObject["Outputs"];

                    foreach (var output in outputObject)
                    {
                        string outputID = output["Id"].ToString();
                        IODictionary.Add(outputID, stringnodeID);                              
                    }
                    JToken inputObject = nodeObject["Inputs"];

                    foreach (var inputs in inputObject)
                    {
                        string inputID = inputs["Id"].ToString();
                        IODictionary.Add(inputID, stringnodeID);
                    }
                    Console.WriteLine(IODictionary);

                }
                

                foreach (var connector in connectorObject)
                {
                    string inputID = connector["Start"].ToString();
                    string outputID = connector["End"].ToString();

                    string NodeAID = IODictionary[ inputID ];
                    string NodeBID = IODictionary[ outputID ];

                    string NodeASig = NodeDictionary[ NodeAID ];
                    string NodeBSig = NodeDictionary[ NodeBID ];

                    Console.WriteLine( NodeAID );
                    Console.WriteLine( NodeBID );
                    Console.WriteLine( NodeASig );
                    Console.WriteLine( NodeBSig );
                    Console.Writeline("************");
               
                    //List StartList = List<connector["Start"].ToString)>;
                }
=======

                foreach (JToken test in nodeObject)
                {
                    string jTokenString = (string)test["FunctionSignature"];
                    Console.WriteLine(jTokenString);
>>>>>>> c5a6c19f0124d36f86b7ac3e46c82bef759b36af

                    //Console.WriteLine(test["FunctionSignature"]);
                }
                ;
                //foreach (JToken thing in nodeObject.Values())
                //{
                //    Console.WriteLine(thing["FunctionSignature"]);
                //    Console.WriteLine(("________"));
                //}

                // Sample JSON query
                //IEnumerable<JToken> jsonQueryEnumerable = from JToken thing in roomObject.Values()
                //    where thing["RoomNumber"].ToString() == "206"
                //    select thing;

                //JToken jsonQuerySingle = jsonQueryEnumerable.First();

                //Console.WriteLine(jsonQuerySingle.ToString());
            }
            Console.Read();
        }


        public class DataParse
        {
            public StringBuilder csvcontent;

            public DataParse()
            {
                csvcontent = InitializeCsvContent();
            }

            public StringBuilder InitializeCsvContent()
            {
                StringBuilder csvcontent = new StringBuilder();
                csvcontent.AppendLine("Node A Name,Node B Name,Node A ID,Node B ID");
                return csvcontent;
            }

            public void AppendToCsv(string nodeAName, string nodeBName, string nodeAId, string nodeBId)
            {
                string csvLine = string.Format("{0},{1},{2},{3}",
                    nodeAName, nodeBName, nodeAId, nodeBId);

                csvcontent.AppendLine(csvLine);
            }

            public void ExportCSV()
            {
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllText(dirPath + "\\graphData.csv", csvcontent.ToString());
            }

        }
    }
}
