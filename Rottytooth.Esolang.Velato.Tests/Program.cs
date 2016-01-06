using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rottytooth.Esolang.Velato;

namespace Rottytooth.Esolang.Velato.Tests
{
    [TestClass]
    class Program
    {
        static void Main(string[] args)
        {
            // These only test whether the programs build, not whether they perform as expected

            //BuildHelloWorld();

            BuildHelloWorldChords();
        }

        [TestMethod]
        static void BuildHelloWorld()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Lilypond/print_hello_world.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "HelloWorld");
            Assert.IsTrue(codeGenerator.GenerateFile());
        }

        [TestMethod]
        static void BuildHelloWorldChords()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Lilypond/print_hello_world_chords.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "HelloWorldChords");
            codeGenerator.GenerateFile();
            Assert.IsTrue(codeGenerator.GenerateFile());
        }
    }
}
