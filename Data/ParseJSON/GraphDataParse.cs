using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using Console = System.Console;

namespace ParseJSON
{
    public class GraphDataParse
    {
        public StringBuilder csvcontent;
        public Regex regex;

        public GraphDataParse()
        {
            csvcontent = InitializeCsvContent();
            regex = CreateRegexPattern();
        }

        public Regex CreateRegexPattern()
        {
            Regex pattern = new Regex("[;,\t\r ]|[\n]{2}");

            return pattern;
        }

        public StringBuilder InitializeCsvContent()
        {
            StringBuilder csvcontent = new StringBuilder();
            csvcontent.AppendLine("Node A Name,Node B Name,Node A ID,Node B ID");
            return csvcontent;
        }

        public void AppendToCsv(string nodeAName, string nodeBName, string nodeAId, string nodeBId)
        {
            string cleanNameA = regex.Replace(nodeAName, "_");
            string cleanNameB = regex.Replace(nodeBName, "_");
            string cleanIdA = regex.Replace(nodeAId, "_");
            string cleanIdB = regex.Replace(nodeBId, "_");

            string csvLine = string.Format("{0},{1},{2},{3}",
                cleanNameA, cleanNameB, cleanIdA, cleanIdB);

            csvcontent.AppendLine(csvLine);
        }

        public void ExportCSV()
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllText(dirPath + "\\graphData.csv", csvcontent.ToString());
        }

        public static void ReadGraphCSV()
        {
            // GLOBALS
            NodeDataContainer nodeDataContainer = new NodeDataContainer();
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = dirPath + "\\graphData.csv";

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    Console.WriteLine(nodeDataContainer.DataModels.Count);

                    if (nodeDataContainer.DataModels.Count == 0)
                    {
                        NodeDataModel newModel =
                            NodeDataModel.CreateNewDataModel(fields[0], fields[1], fields[3], fields[2]);
                        nodeDataContainer.AppendToDataContainer(newModel);
                    }
                    else
                    {
                        IEnumerable<NodeDataModel> existingRecords = from record in nodeDataContainer.DataModels
                                                                        where record.NodeAId == fields[2]
                                                                        select record;
                        if (existingRecords.Count() == 0)
                        {
                            NodeDataModel newModel =
                                NodeDataModel.CreateNewDataModel(fields[0], fields[1], fields[3], fields[2]);
                            nodeDataContainer.AppendToDataContainer(newModel);
                        }
                        else
                        {
                            //Console.WriteLine(existingRecords.Count());
                            NodeDataModel exisingRecord = existingRecords.First();
                            exisingRecord.TotalConnectionsCount = +1;

                            if (exisingRecord.NodeBId != fields[3])
                            {
                                exisingRecord.UniqueConnectionsCount = +1;
                            }
                        }
                    }

                    //if (nodeDataContainer.DataModels.Count > 10)
                    //{
                    //    Console.WriteLine("TEN");
                    //}
                }

                var container = nodeDataContainer.DataModels;
                Console.WriteLine(nodeDataContainer.DataModels.Count);

                Console.ReadKey();
            }

            // NEW CSV SHIT
            StringBuilder csvcontent = new StringBuilder();
            csvcontent.AppendLine("Node A Name,Node B Name,# Connections,# Connections Unique,Type");
            Regex pattern = new Regex("^([^.]+)");

            foreach (NodeDataModel nd in nodeDataContainer.DataModels)
            {
                List<string> matchStrings = new List<string>(); //Create empty container
                MatchCollection matches = pattern.Matches(nd.NodeAName); //Get all match occurances in a document

                string resultString = "EMPTY"; //Set default value for the string to placate MSVS

                // Try to get node types
                foreach (Match match in matches) //Loop over all regex matches found
                {
                    if (match.Success) //If the match is successful
                    {
                        resultString = match.Value;
                        matchStrings.Add(resultString); 
                    }
                }



                // Create csv row
                string csvLine = string.Format("{0},{1},{2},{3},{4}",
                   nd.NodeAName, nd.NodeBName, nd.TotalConnectionsCount, nd.UniqueConnectionsCount, resultString);

                csvcontent.AppendLine(csvLine);
            }

            File.WriteAllText(dirPath + "\\graphDataFeaturized.csv", csvcontent.ToString());



        }

    }

}
