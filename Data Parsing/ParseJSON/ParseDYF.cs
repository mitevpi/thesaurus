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
        public static List<string> GetDyfsInDir(string path)
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
                        List<string> matches = GetMatches(lines);

                        try
                        {
                            Console.WriteLine(matches[0]);
                        }
                        catch
                        {
                        }


                        //Console.WriteLine(f);
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

        public static void ParseDyfData(List<string> filePaths)
        {
            // DEFINE GLOBALS
            DataParse csvParser = new DataParse();

            foreach (var fileName in filePaths) //Loop over all the files in teh directory
            {
                string dyfTitle = System.IO.Path.GetFileName(fileName);
                ////dyfTitle = dyfTitle.Remove(dyfTitle.Length - 4);
                //Console.WriteLine(dyfTitle);

                string[] temp2 = System.IO.File.ReadAllLines(fileName);
                string temp3 = temp2[0];
                //Console.WriteLine(temp3);

                //List<string> matches = GetMatches(temp3);
                //Console.WriteLine(matches);

                //txtFirstLine.Text = lines(0)
                //txtLastLine.Text = lines(lines.Length - 1)


                //string temp = File.ReadLines(fileName).First();
                //Console.WriteLine("");
                //Console.WriteLine(temp);

                using (StreamReader reader = File.OpenText(fileName))
                {
                    var temp = reader.ReadLine();
                    //Console.WriteLine(temp);
                }
            }
        }

        public static Regex CreateIDRegex()
        {
            string sample = "ID = 6c712c67-329c-4499-a399-d1cedd2b45bf";


            string dash = "(-)"; // Dash
            string id = "(ID=)"; // LETTERS
            string chars = "([0-9a-fA-F]+)";

            Regex reg = new Regex(@"ID=.[0-9a-fA-F]+-[0-9a-fA-F]+-[0-9a-fA-F]+-[0-9a-fA-F]+-[0-9a-fA-F]+.");
            Regex reg2 = new Regex(id);


            return reg;
        }

        public static List<string> GetMatches(string fileString)
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

    }
}
