using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rottytooth.Esolang.Velato.UnitTests
{
    [TestClass]
    public class CompilerTests
    {
        // FIXME: these should first re-compile the lilypond files to midi

        [TestMethod]
        public void BuildHelloWorld()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Programs/print_hello_world.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "HelloWorld");
            Assert.IsTrue(codeGenerator.GenerateFile());

            string code = codeGenerator.GenerateCSharp();
            Assert.IsTrue(code.Contains("Console.Write('H');"));
            Assert.IsTrue(code.Contains("Console.Write('e');"));
            Assert.IsTrue(code.Contains("Console.Write('l');"));
            Assert.IsTrue(code.Contains("Console.Write('o');"));
            Assert.IsTrue(code.Contains("Console.Write('W');"));
            Assert.IsTrue(code.Contains("Console.Write('d');"));
            Assert.IsTrue(code.Contains("Console.Write('!');"));
            Assert.IsTrue(code.Contains("Console.Write(',');"));
            Assert.IsTrue(code.Contains("Console.Write(' ');"));
        }

        [TestMethod]
        public void BuildHelloWorldChords()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Programs/print_hello_world_chords.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "HelloWorldChords");
            codeGenerator.GenerateFile();
            Assert.IsTrue(codeGenerator.GenerateFile());
        }

        [TestMethod]
        public void BuildFugue()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Programs/fugue.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "Fugue");
            codeGenerator.GenerateFile();
            Assert.IsTrue(codeGenerator.GenerateFile());

            string code = codeGenerator.GenerateCSharp();
            code = Regex.Replace(code, @"\s", "");
            Assert.IsTrue(code.Contains("charA457;"));
            Assert.IsTrue(code.Contains("A457='-';"));
            Assert.IsTrue(code.Contains("intE564;"));
            Assert.IsTrue(code.Contains("E564=8;"));
            Assert.IsTrue(code.Contains("Fs454=8;"));
            Assert.IsTrue(code.Contains("Console.Write(A457);"));
            Assert.IsTrue(code.Contains("Console.Write(E564);"));
        }

        [TestMethod]
        public void BuildWhile()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Programs/while_test.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "Fugue");
            codeGenerator.GenerateFile();
            Assert.IsTrue(codeGenerator.GenerateFile());

            string code = codeGenerator.GenerateCSharp();
            string codeWithoutSpaces = Regex.Replace(code, @"\s", "");
            Assert.IsTrue(codeWithoutSpaces.Contains("while(E452>0)"));
            Assert.IsTrue(codeWithoutSpaces.Contains("Console.Write(E452);"));
        }

        [TestMethod]
        public void BuildHe()
        {
            MidiLoader loader = new MidiLoader();
            loader.Load("../../../Programs/print_he.mid");

            Parser parser = new Parser(loader.Notes, loader.SmallestInterval);
            CodeGenerator codeGenerator = new CodeGenerator(parser.Parse(), "He");
            codeGenerator.GenerateFile();
            Assert.IsTrue(codeGenerator.GenerateFile());

            string code = codeGenerator.GenerateCSharp();
            Assert.IsTrue(code.Contains("Console.Write('H');"));
            Assert.IsTrue(code.Contains("Console.Write('e');"));
        }
    }
}
