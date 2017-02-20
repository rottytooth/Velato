using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rottytooth.Esolang.Velato.Converter.Model;

namespace Rottytooth.Esolang.Velato.UnitTests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void TestIntervals()
        {
            Converter.Model.Note result;

            result = Converter.NoteUtil.GetNoteByInterval(new Converter.Model.Note() { Name = NoteName.C }, Interval.MinorSixth);
            Assert.AreEqual(result.Name, NoteName.A);
            Assert.AreEqual(result.Accidental, Accidental.Flat);

            result = Converter.NoteUtil.GetNoteByInterval(new Converter.Model.Note() { Name = NoteName.F }, Interval.DiminishedFifth);
            Assert.AreEqual(result.Name, NoteName.C);
            Assert.AreEqual(result.Accidental, Accidental.Flat);

            result = Converter.NoteUtil.GetNoteByInterval(new Converter.Model.Note() { Name = NoteName.F }, Interval.PerfectFourth);
            Assert.AreEqual(result.Name, NoteName.B);
            Assert.AreEqual(result.Accidental, Accidental.Flat);

            result = Converter.NoteUtil.GetNoteByInterval(new Converter.Model.Note() { Name = NoteName.D, Accidental = Accidental.DoubleFlat }, Interval.MinorThird);
            Assert.AreEqual(result.Name, NoteName.F);
            Assert.AreEqual(result.Accidental, Accidental.DoubleFlat);

            // undocumented triple flat
            result = Converter.NoteUtil.GetNoteByInterval(new Converter.Model.Note() { Name = NoteName.D, Accidental = Accidental.DoubleFlat }, Interval.MinorSixth);
            Assert.AreEqual(result.Name, NoteName.B);
            Assert.AreEqual(result.Accidental, Accidental.TripleFlat);

            result = Converter.NoteUtil.GetNoteByInterval(new Converter.Model.Note() { Name = NoteName.D, Accidental = Accidental.DoubleSharp }, Interval.MajorSixth);
            Assert.AreEqual(result.Name, NoteName.B);
            Assert.AreEqual(result.Accidental, Accidental.DoubleSharp);
        }

        public string HelloWorldTestString =
            "Starting Note [E]\n" +
            "Print['H']\n" +
            "Print['e']\n" +
            "Print['l']\n" +
            "Print['l']\n" +
            "Print['o']\n" +
            "Print[',']\n" +
            "Print[' ']\n" +
            "Print['W']\n" +
            "Print['o']\n" +
            "Print['r']\n" +
            "Print['l']\n" +
            "Print['d']\n" +
            "Print['!']";

        [TestMethod]
        public void TestHelloWorld()
        {
            Converter.PseudoToVelato parser = new Converter.PseudoToVelato();
            parser.Convert(HelloWorldTestString);
        }
    }
}
