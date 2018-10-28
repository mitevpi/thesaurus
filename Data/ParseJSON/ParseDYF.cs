using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParseJSON
{
    public class ParseDYF
    {
        public StringBuilder csvcontent;
        public Regex CleanupRegex;
        public Dictionary<string, string> customPackageMappings;

        public ParseDYF()
        {
            csvcontent = InitializeCsvContent();
            CleanupRegex = CreateCleanupRegex();
            customPackageMappings = new Dictionary<string, string>();
        }

        public List<string> GetDyfsInDir(string path)
        {
            List<string> files = new List<string>();
            try
            {
                foreach (var d in Directory.GetDirectories(path))
                {
                    Console.WriteLine(d);
                    foreach (var f in Directory.GetFiles(d))
                    {
                        files.Add(f);
                        //Console.WriteLine(f);

                        // Filter out all the DYF files
                        if (f.EndsWith("dyf", StringComparison.OrdinalIgnoreCase))
                            files.Add(f);

                        string[] fullText = System.IO.File.ReadAllLines(f);
                        string lines = fullText[0];
                        List<string> IdMatches = GetIdMatches(lines);
                        List<string> NameMatches = GetNameMatches(lines);

                        try
                        {
                            // ID REGEX
                            string matchId = IdMatches[0];
                            matchId = matchId.Replace("\"", "");
                            matchId = matchId.Replace("ID=", "");

                            // NAME REGEX
                            string matchName = NameMatches[0];
                            matchName = matchName.Replace("\"", "");
                            matchName = matchName.Replace("Name=", "");

                            // CSV WRITE
                            string csvLine = matchId + "," + matchName + "," + f;
                            csvcontent.AppendLine(csvLine);
                            customPackageMappings.Add(matchId, matchName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                    GetDyfsInDir(d);
                }
            }
            catch (Exception)
            {
                // ignore 
            }

            Console.WriteLine("FINISHED PARSING");

            return files;
        }

        public StringBuilder InitializeCsvContent()
        {
            StringBuilder csvcontent = new StringBuilder();
            csvcontent.AppendLine("GUID,Name,Path");
            return csvcontent;
        }

        public void ExportCSV()
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllText(dirPath + "\\packageData.csv", csvcontent.ToString());
        }
        

        public static Regex CreateIDRegex()
        {
            string sample = "ID = 6c712c67-329c-4499-a399-d1cedd2b45bf";
            string dash = "(-)"; // Dash
            string id = "(ID=)"; // LETTERS
            string chars = "([0-9a-fA-F]+)";

            Regex reg = new Regex(@"ID=.[0-9a-fA-F]+-[0-9a-fA-F]+-[0-9a-fA-F]+-[0-9a-fA-F]+-[0-9a-fA-F]+.");

            return reg;
        }

        public static Regex CreateNameRegex()
        {
            Regex reg = new Regex(@""" Name="".+?(?=\"")");

            return reg;
        }

        public Regex CreateCleanupRegex()
        {
            string patternString = @"""([^""]|"""")*""";
            //string quote = @"(")";

            Regex pattern = new Regex(patternString);

            return pattern;
        }

        public static List<string> GetIdMatches(string fileString)
        {
            List<string> matchStrings = new List<string>(); //Create empty container

            Regex reg = CreateIDRegex();
            MatchCollection matches = reg.Matches(fileString); //Get all match occurances in a document

            string resultString = "EMPTY"; //Set default value for the string to placate MSVS

            foreach (Match match in matches) //Loop over all regex matches found
            {
                if (match.Success) //If the match is successful
                {
                    resultString = match.Value; //Get the text of the matched KTMS code
                    //Console.WriteLine(match.Value);
                    matchStrings.Add(resultString); //Add the text to the list of matched KTMS codes
                }
            }

            return matchStrings;

        }

        public static List<string> GetNameMatches(string fileString)
        {
            List<string> matchStrings = new List<string>(); //Create empty container

            Regex reg = CreateNameRegex();
            MatchCollection matches = reg.Matches(fileString); //Get all match occurances in a document

            string resultString = "EMPTY"; //Set default value for the string to placate MSVS

            foreach (Match match in matches) //Loop over all regex matches found
            {
                if (match.Success) //If the match is successful
                {
                    resultString = match.Value; //Get the text of the matched KTMS code
                    //Console.WriteLine(match.Value);
                    matchStrings.Add(resultString); //Add the text to the list of matched KTMS codes
                }
            }

            return matchStrings;

        }

    }
}
