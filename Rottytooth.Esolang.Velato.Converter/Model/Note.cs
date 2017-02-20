using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Rottytooth.Esolang.Velato.Converter.Model
{
    public class Note
    {
        public NoteName Name { get; set; }

        public Accidental Accidental { get; set; }

        public int? Octave { get; set; }

        public Note()
        {
            this.Accidental = Accidental.Natural;
        }

        public Note(NoteName name)
        {
            this.Name = name;
            this.Accidental = Accidental.Natural;
        }

        public Note(NoteName name, Accidental acc)
        {
            this.Name = name;
            this.Accidental = acc;
        }

        public static Note FromString(string notestr)
        {
            Note retNote = new Note();
            retNote.Accidental = Accidental.Natural;
            if (notestr.Contains("b"))
            {
                retNote.Accidental = Accidental.Flat;
                if (notestr.Contains("bb"))
                {
                    retNote.Accidental = Accidental.DoubleFlat;
                }
            }
            else if (notestr.Contains("#"))
            {
                retNote.Accidental = Accidental.Sharp;
                if (notestr.Contains("##"))
                {
                    retNote.Accidental = Accidental.DoubleSharp;
                }
            }
            retNote.Name = (NoteName)Enum.Parse(typeof(NoteName), 
                notestr[0].ToString().ToUpper(), true);

            if (Regex.IsMatch(notestr, ".*\\d+"))
            {
                retNote.Octave = Int32.Parse(Regex.Match(notestr, ".*(\\d+)").Groups[1].Value);
            }

            return retNote;
        }

        public override string ToString()
        {
            string friendlyAccidental = "";

            switch(Accidental)
            {
                case Model.Accidental.TripleFlat:
                    friendlyAccidental = "bbb";
                    break;
                case Model.Accidental.DoubleFlat:
                    friendlyAccidental = "bb";
                    break;
                case Model.Accidental.Flat:
                    friendlyAccidental = "b";
                    break;
                case Model.Accidental.Sharp:
                    friendlyAccidental = "#";
                    break;
                case Model.Accidental.DoubleSharp:
                    friendlyAccidental = "x";
                    break;
                case Model.Accidental.TripleSharp:
                    friendlyAccidental = "x#";
                    break;
            }

            return Name.ToString() + friendlyAccidental + (Octave != null ? Octave.ToString() : "");
        }
    }
}
