using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CSharp;
using Rottytooth.Esolang.Velato.Converter.Model;

namespace Rottytooth.Esolang.Velato.Converter
{
    public class PseudoToVelato
    {
        private Note RootNote { get; set; }

        private int CurrentLine { get; set; }

        private string CurrentCommand { get; set; }

        public List<OutputNote> Convert(string commands)
        {
            var retNotes = new List<OutputNote>();


            string[] commandList = commands.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (CurrentLine = 0; CurrentLine < commandList.Length; CurrentLine++)
            {
                CurrentCommand = commandList[CurrentLine];
                string commandName = CurrentCommand;

                if (CurrentCommand.Contains("["))
                {
                    commandName = CurrentCommand.Split('[')[0].Trim();
                }


                string[] args = null;
                if (CurrentCommand.Split('[').Length > 1)
                {
                    // match just what's in the square brackets
                    // then split on comma for multiple entries in args; don't match commas in quotes
                    string argsUnformatted = CurrentCommand.Split('[')[1].Split(']')[0];
                    args = Regex.Split(argsUnformatted,@"(?<=^([^""]|""[^""]*"")*),");

                    for (int a = 0; a < args.Length; a++)
                    {
                        if (args[a].Contains(":"))
                        {
                            args[a] = args[a].Split(':')[1].Trim();
                        }
                    }
                }

                switch (commandName.Trim().ToLower())
                {
                    case "starting note":
                        // just one note
                        if (args.Length < 1) throw new SyntaxError("No Starting Note provided", CurrentLine, CurrentCommand);
                        RootNote = Note.FromString(args[0]);
                        retNotes.Add(new OutputNote(RootNote, CurrentLine, CurrentCommand));
                        break;
                    case "change root note":
                        // a Major Second and then the new note
                        retNotes.Add(new OutputNote(
                            NoteUtil.GetNoteByInterval(RootNote, Interval.MajorSecond),
                            CurrentLine, CurrentCommand));
                        if (args.Length < 1) throw new SyntaxError("No New Root Note provided", CurrentLine, CurrentCommand);
                        RootNote = Note.FromString(args[0]);
                        retNotes.Add(new OutputNote(RootNote, CurrentLine, CurrentCommand));
                        break;
                    case "let":
                        // a minor third, then a variable (note) and then an expression
                        retNotes.Add(new OutputNote(NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird), CurrentLine, CurrentCommand));
                        if (args.Length < 1) throw new SyntaxError("No Variable Pitch provided", CurrentLine, CurrentCommand);
                        retNotes.Add(new OutputNote(Note.FromString(args[0]), CurrentLine, CurrentCommand));
                        if (args.Length < 2) throw new SyntaxError("No expression provided for Let", CurrentLine, CurrentCommand);
                        ParseExpression(args[1], retNotes);
                        break;
                    case "declare":
                        // a minor sixth, then a variable (note) and then a type
                        retNotes.Add(new OutputNote(
                            NoteUtil.GetNoteByInterval(RootNote, Interval.MinorSixth), 
                            CurrentLine, CurrentCommand));
                        if (args.Length < 1) throw new SyntaxError("No Variable Pitch provided", CurrentLine, CurrentCommand);
                        retNotes.Add(new OutputNote(Note.FromString(args[0]), CurrentLine, CurrentCommand));
                        if (args.Length < 2) throw new SyntaxError("No type provided for Declare" , CurrentLine, CurrentCommand);

                        switch(args[1].Trim().ToLower())
                        {
                            case "int":
                                // a second
                                retNotes.Add(
                                    new OutputNote(
                                        new List<Note>() {
                                        NoteUtil.GetNoteByInterval(RootNote, Interval.MinorSecond),
                                        NoteUtil.GetNoteByInterval(RootNote, Interval.MajorSecond)
                                    }, CurrentLine, CurrentCommand));
                                break;
                            case "char":
                                // a third
                                retNotes.Add(
                                    new OutputNote(
                                        new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird)
                                }, CurrentLine, CurrentCommand));
                                break;
                            case "double":
                                // a fourth
                                retNotes.Add(
                                    new OutputNote(
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.PerfectFourth),
                                    CurrentLine, CurrentCommand));
                                break;
                        }
                        break;
                    case "while":
                        // a major third, another major third, then an expression
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird), CurrentLine, CurrentCommand));
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird), CurrentLine, CurrentCommand));
                        if (args.Length < 1) throw new SyntaxError("No expression provided for While", CurrentLine, CurrentCommand);
                        ParseExpression(args[0], retNotes);
                        break;
                    case "end while":
                        // a major third, a perfect fourth
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird), CurrentLine, CurrentCommand));
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.PerfectFourth), CurrentLine, CurrentCommand));
                        break;
                    case "if":
                        // a major third, a perfect fifth, then an expression
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird), CurrentLine, CurrentCommand));
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.PerfectFifth), CurrentLine, CurrentCommand));
                        if (args.Length < 1) throw new SyntaxError("No expression provided for While", CurrentLine, CurrentCommand);
                        ParseExpression(args[0], retNotes);
                        break;
                    case "else":
                        // a major third, a major sixth
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird), CurrentLine, CurrentCommand));
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorSixth), CurrentLine, CurrentCommand));
                        break;
                    case "end if":
                        // a major third, a major seventh
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird), CurrentLine, CurrentCommand));
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorSeventh), CurrentLine, CurrentCommand));
                        break;
                    case "print":
                        // a major sixth, a perfect fifth, then an expression
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.MajorSixth), CurrentLine, CurrentCommand));
                        retNotes.Add(new OutputNote( NoteUtil.GetNoteByInterval(RootNote, Interval.PerfectFifth), CurrentLine, CurrentCommand));
                        if (args.Length < 1) throw new SyntaxError("No expression provided for Print", CurrentLine, CurrentCommand);
                        ParseExpression(args[0], retNotes);
                        break;
                }
            }

            return retNotes;
        }

        private void ParseExpression(string expression, List<OutputNote> retNotes)
        {
            expression = Regex.Replace(expression, "\"", "'");

            string[] expPieces = expression.Split(null); // splits on whitespace

            foreach(string exp in expPieces)
            {
                if ((exp.Trim().StartsWith("'") && exp.Trim().EndsWith("'")) ||
                    (exp.Trim().StartsWith("\"") && exp.Trim().EndsWith("\"")))
                {
                    // a char
                    if (exp.Trim().Length != 3)
                    {
                        throw new SyntaxError("Character can only have one symbol", CurrentLine, CurrentCommand);
                    }
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird)
                    }, CurrentLine, CurrentCommand));
                    retNotes.Add(
                        new OutputNote(
                            NoteUtil.GetNoteByInterval(RootNote, Interval.PerfectFourth), CurrentLine, CurrentCommand));

                    foreach (Interval interval in GetIntervalsForInt(exp[1]))
                    {
                        retNotes.Add(
                            new OutputNote(
                                NoteUtil.GetNoteByInterval(RootNote, interval), CurrentLine, CurrentCommand)
                        );
                    }
                    continue;
                }
                if (Regex.IsMatch(exp.Trim(), "[A-G][b#x]*", RegexOptions.IgnoreCase))
                {
                    //a variable
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird)
                    }, CurrentLine, CurrentCommand));
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorSecond),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorSecond)
                    }, CurrentLine, CurrentCommand));
                    retNotes.Add(new OutputNote(Note.FromString(exp), CurrentLine, CurrentCommand));
                    continue;
                }
                if (Regex.IsMatch(exp.Trim(), @"$\d+^"))
                {
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird)
                    }, CurrentLine, CurrentCommand));
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.PerfectFifth),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.DiminishedFifth)
                    }, CurrentLine, CurrentCommand));

                    foreach (Interval interval in GetIntervalsForInt(int.Parse(exp)))
                    {
                        retNotes.Add(
                            new OutputNote(
                                NoteUtil.GetNoteByInterval(RootNote, interval), CurrentLine, CurrentCommand)
                        );
                    }
                    continue;
                }
                if (Regex.IsMatch(exp.Trim(), @"$-\d+^"))
                {
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird)
                    }, CurrentLine, CurrentCommand));
                    retNotes.Add(
                        new OutputNote(
                            new List<Note>() {
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MinorThird),
                                    NoteUtil.GetNoteByInterval(RootNote, Interval.MajorThird)
                    }, CurrentLine, CurrentCommand));

                    foreach (Interval interval in GetIntervalsForInt(int.Parse(exp)))
                    {
                        retNotes.Add(
                            new OutputNote(
                                NoteUtil.GetNoteByInterval(RootNote, interval), CurrentLine, CurrentCommand)
                        );
                    }
                    continue;
                }
            }
        }

        List<Interval> GetIntervalsForInt(int val)
        {
            List<Interval> retList = new List<Interval>();
            int[] intArray = GetIntArray(val);
            foreach (int _int in intArray)
            {
                if (_int < 7) // do this to avoid PerfectFifth
                {
                    retList.Add((Interval)(_int + 1));
                }
                else
                {
                    retList.Add((Interval)(_int + 2));
                }
            }
            retList.Add(Interval.PerfectFifth); // always ends with this value
            return retList;
        }

        int[] GetIntArray(int num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }
    }
}
