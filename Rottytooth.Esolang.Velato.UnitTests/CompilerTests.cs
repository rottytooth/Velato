using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rottytooth.Esolang.Velato.UnitTests
{
    [TestClass]
    public class CompilerTests
    {
        // NOTE: These only test whether the programs build, not whether they perform as expected

        [TestMethod]
        public void BuildHelloWorld()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Lilypond/print_hello_world.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "HelloWorld");
            Assert.IsTrue(codeGenerator.GenerateFile());
        }

        [TestMethod]
        public void BuildHelloWorldChords()
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
