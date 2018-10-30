using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ParseJSON
{
    public class DataParse
    {
        public StringBuilder csvcontent;
        public Regex regex;

        public DataParse()
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

    }
}
