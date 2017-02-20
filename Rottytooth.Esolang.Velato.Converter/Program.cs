using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Rottytooth.Esolang.Velato.Converter.Model;

namespace Rottytooth.Esolang.Velato.Converter
{
    // Takes psuedo-code and gives back notes for a Velato program

    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PseudoToVelato parser = new PseudoToVelato();
                List<OutputNote> song = parser.Convert(ReadFile("../../../PseudoFiles/HelloWorld.txt"));

                foreach (OutputNote line in song)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static string ReadFile(string path)
        {
            string text = "";
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(path))
                {
                    text = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return text;
        }
    }
}
