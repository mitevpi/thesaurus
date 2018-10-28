using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParseJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string packagePath = appDataPath + "\\" + "Dynamo" + "\\" + "Dynamo Revit" + "\\" + "2.1" + "\\" + "packages";
            Regex reg = ParseDYF.CreateIDRegex();
            ParseDYF dyfObj = new ParseDYF();

            // DYNAMO PACKAGES PARSE
            List<string> paths = dyfObj.GetDyfsInDir(packagePath);
            dyfObj.ExportCSV();

            // DYNAMO GRAPH PARSE
            ParseJSON jsonObj = new ParseJSON();
            jsonObj.ParseJSONs(dyfObj);
            Console.ReadKey();

        }
    }
}
