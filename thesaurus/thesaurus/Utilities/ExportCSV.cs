using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ParseJSON
{
    public class DataParse
    {
        private StringBuilder csvContent;
        private Regex regex;

        public DataParse()
        {
            csvContent = InitializeCsvContent();
            regex = CreateRegexPattern();
        }

        /// <summary>
        /// Creates a Regex pattern used to populate CSV rows.
        /// </summary>
        /// <returns>Pattern.</returns>
        private static Regex CreateRegexPattern()
        {
            var pattern = new Regex("[;,\t\r ]|[\n]{2}");

            return pattern;
        }

        /// <summary>
        /// Builds CSV File Headers.
        /// </summary>
        /// <returns>String Builder with CSV Headers.</returns>
        private static StringBuilder InitializeCsvContent()
        {
            var c = new StringBuilder();
            c.AppendLine("Node A Name,Node B Name,Node A ID,Node B ID");
            return c;
        }

        /// <summary>
        /// Appends new line in CSV File that has info about given node pair.
        /// </summary>
        /// <param name="nodeAName">Name A</param>
        /// <param name="nodeBName">Name B</param>
        /// <param name="nodeAId">Id A</param>
        /// <param name="nodeBId">Id B</param>
        public void AppendToCsv(string nodeAName, string nodeBName, string nodeAId, string nodeBId)
        {
            var cleanNameA = regex.Replace(nodeAName, "_");
            var cleanNameB = regex.Replace(nodeBName, "_");
            var cleanIdA = regex.Replace(nodeAId, "_");
            var cleanIdB = regex.Replace(nodeBId, "_");

            var csvLine = $"{cleanNameA},{cleanNameB},{cleanIdA},{cleanIdB}";

            csvContent.AppendLine(csvLine);
        }

        /// <summary>
        /// Saves CSV to local drive.
        /// </summary>
        public void ExportCSV()
        {
            var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllText(dirPath + "\\graphData.csv", csvContent.ToString());
        }
    }
}