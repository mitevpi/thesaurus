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
            string filePath = dirPath + "\\dynamoTest.json";

            using (StreamReader reader = File.OpenText(filePath))
            {
                // Read JSON file, and get a JToken from it for iterating
                JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                JToken nodeObject = jObject["Nodes"];

                foreach (var test in nodeObject)
                {
                    //Console.WriteLine(test);
                    Console.WriteLine(test["FunctionSignature"]);
                }

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
                csvcontent.AppendLine("Node A,Node B,Inputs,Outputs");
                return csvcontent;
            }

            public void AppendToCsv(string nodeA, string nodeB, string inputNodes, string outputNodes)
            {

                csvcontent.AppendLine(nodeA, nodeB, inputNodes, outputNodes);
            }

            public void ExportCSV()
            {
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllText(dirPath + "\\graphData.csv", csvcontent.ToString());

            }

        }
    }
}
