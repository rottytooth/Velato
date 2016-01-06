using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rottytooth.Esolang.Velato.Tokens;

namespace Rottytooth.Esolang.Velato
{
    class Program
    {
        static string Path { get; set; }

        /// <summary>
        /// Output as Javascript file
        /// </summary>
        static bool ToJavascript { get; set; }

        /// <summary>
        /// Compile to exe
        /// </summary>
        static bool Compile { get; set; }

        /// <summary>
        /// Output as a C Sharp file
        /// </summary>
        static bool ToCSharp { get; set; }

        /// <summary>
        /// Print options and then end
        /// </summary>
        static bool PrintHelp { get; set; }

        private const string HelpText = "";

        static void Main(string[] args)
        {
            MidiLoader loader = new MidiLoader();

            GetPathAndOptions(args);

            FileInfo codeFile = new FileInfo(Path);
            string programName = codeFile.Name;

            loader.Load(Path);

            foreach (Note note in loader.Notes)
            {
                Console.WriteLine(note.Name + "\t" + note.Number.ToString());

                Console.WriteLine(Math.Round(note.PitchCorrection * loader.SmallestInterval) + "/" + loader.SmallestInterval.ToString());

            }
            Console.WriteLine("smallest tone: 1/" + loader.SmallestInterval.ToString());

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            List<CommandToken> tokens = parser.Parse();

            CodeGenerator codeGenerator = new CodeGenerator(tokens, programName);

            if (ToJavascript)
            {
                FileInfo jsFile = new FileInfo(programName + ".js");
                using (StreamWriter writer = jsFile.CreateText())
                {
                    writer.Write(codeGenerator.GenerateJavaScript());
                    writer.Flush();
                }
            }
            if (ToCSharp)
            {
                FileInfo csFile = new FileInfo(programName + ".cs");
                using (StreamWriter writer = csFile.CreateText())
                {
                    writer.Write(codeGenerator.GenerateCSharp());
                    writer.Flush();
                }
            }
            if (Compile)
            {
                codeGenerator.GenerateFile();
            }
        }

        static void GetPathAndOptions(string[] args)
        {
            // defaults
            PrintHelp = false;
            ToJavascript = false;
            ToCSharp = false;
            Compile = true;

            bool hasPath = false;

            foreach (string arg in args)
            {
                if (arg[0] == '-' || arg[0] == '/')
                {
                    // is an option

                    if (arg[1] == '?')
                    {
                        PrintHelp = true;
                    }
                    if (arg[1] == 's' || arg[1] == 'S') // S for C# string output
                    {
                        Compile = false;
                        ToCSharp = true;
                    }
                    if (arg[1] == 'j' || arg[1] == 'J')
                    {
                        Compile = false;
                        ToJavascript = true;
                    }
                    if (arg[1] == 'c' || arg[1] == 'C') // compile even though there's additionally an S or J arg
                    {
                        Compile = true;
                    }
                }
                else
                {
                    if (!hasPath)
                    {
                        Path = arg;
                        hasPath = true;
                    }
                }
            }
        }
    }
}
