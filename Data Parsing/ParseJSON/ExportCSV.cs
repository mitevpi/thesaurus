using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseJSON
{
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
