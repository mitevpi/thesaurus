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
    class NestedSample
    {
        public static void ParseNestedJSON()
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = dirPath + "\\dynamoTest.json";

            using (StreamReader reader = File.OpenText(filePath))
            {
                // Read JSON file, and get a JToken from it for iterating
                JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                JToken nodeObject = jObject["Nodes"];
                JToken connectorObject = jObject["Connectors"];

                List<JToken> nestedTokenList = new List<JToken>();

                foreach (JToken test in nodeObject)
                {
                    JObject tempObj = JObject.Parse(test.ToString());
                    Console.WriteLine(tempObj);
                    JToken nestedObj = tempObj["Outputs"];

                    foreach (JToken test2 in nestedObj)
                    {
                        Console.WriteLine(test2);
                        nestedTokenList.Add(test2);
                    }


                    string jTokenString = (string)test["FunctionSignature"];
                    //Console.WriteLine(jTokenString);

                    //Console.WriteLine(test["FunctionSignature"]);
                }

                foreach (JToken test in connectorObject)
                {
                    string jTokenString = (string)test["Start"];
                    //Console.WriteLine(jTokenString);

                    IEnumerable<JToken> jsonQueryEnumerable = from JToken thing in nestedTokenList
                                                              where thing["Id"].ToString() == "0faed7bef55346c681d5ef3030b97440"
                                                              select thing;

                    Console.WriteLine("SUCCESS??");
                    Console.WriteLine(jsonQueryEnumerable.First());

                    //Console.WriteLine(jsonQueryEnumerable.First().ToString());
                }
            }
            Console.Read();
        }
    }
}
