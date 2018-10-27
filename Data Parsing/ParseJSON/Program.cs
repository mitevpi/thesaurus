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
                //Console.WriteLine("JOBJECT");
                //Console.Write(jObject);

                JToken nodeObject = jObject["Nodes"];
                //Console.WriteLine("NODES");
               // Console.Write(nodeObject);

                //Console.Write(jObject["Nodes"][0]);

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
    }
}
