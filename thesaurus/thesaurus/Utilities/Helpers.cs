using System;
using System.Collections.Generic;
using System.IO;

namespace thesaurus.Utilities
{
    public class Helpers
    {
        /// <summary>
        /// Recursively search the folder to get all the DYN files
        /// Return as list of string
        /// </summary>
        /// <param name="path">Directory path.</param>
        public static List<string> DirSearch(string path)
        {
            var files = new List<string>();
            try
            {
                // (Konrad) Handler sub-directories recursively.
                foreach (var d in Directory.GetDirectories(path))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        // (Konrad) Filter out all the DYN files
                        if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase)) files.Add(f);
                    }
                    DirSearch(d);
                }

                // (Konrad) Handle main directory.
                foreach (var f in Directory.GetFiles(path))
                {
                    if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase)) files.Add(f);
                }
            }
            catch (Exception)
            {
                // ignore 
            }
            return files;
        }
    }
}
