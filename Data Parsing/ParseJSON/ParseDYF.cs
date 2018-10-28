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
                        Console.WriteLine(f);

                        // Filter out all the DYF files
                        if (f.EndsWith("dyf", StringComparison.OrdinalIgnoreCase))
                            files.Add(f);
                        Console.WriteLine(f);
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
                dyfTitle = dyfTitle.Remove(dyfTitle.Length - 4);
                Console.WriteLine(dyfTitle);

                string[] temp2 = System.IO.File.ReadAllLines(fileName);
                var temp3 = temp2[0];
                Console.WriteLine(temp3);

                //txtFirstLine.Text = lines(0)
                //txtLastLine.Text = lines(lines.Length - 1)


                //string temp = File.ReadLines(fileName).First();
                //Console.WriteLine("");
                //Console.WriteLine(temp);

                //using (StreamReader reader = File.OpenText(fileName))
                //{
                //}
            }
        }

        static Regex CreateIDRegex()
        {
            string sample = "ID = 6c712c67-329c-4499-a399-d1cedd2b45bf";

            string re1 = "([A-Z])";	// Uppercase Single Word Character (Not Whitespace) 1
            string re2 = "([A-Z])"; // Uppercase Single Word Character (Not Whitespace) 2
            string re3 = "(-)"; // Dash
            string re4 = "([A-Z])"; // Uppercase Single Word Character (Not Whitespace) 3
            string re5 = "([A-Z])"; // Uppercase Single Word Character (Not Whitespace) 4
            string re6 = "(-)"; // Dash
            string re7 = "(\\d)";   // Any Single Digit 1
            string re8 = "(\\d)";   // Any Single Digit 2

            Regex regKTMS = new Regex(re1 + re2 + re3 + re4 + re5 + re6 + re7 + re8, RegexOptions.Singleline);

            return regKTMS;
        }

    }
}
