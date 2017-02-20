using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rottytooth.Esolang.Velato.Converter.Model
{
    public class OutputNote
    {
        public List<Note> Notes { get; set; }

        public int LineNumber { get; set; }

        public string LineText { get; set; }

        public OutputNote(List<Note> notes, int lineNumber, string lineText)
        {
            this.Notes = notes;
            this.LineNumber = lineNumber;
            this.LineText = lineText;
        }

        public OutputNote(Note note, int lineNumber, string lineText)
        {
            this.Notes = new List<Note>() { note };
            this.LineNumber = lineNumber;
            this.LineText = lineText;
        }

        public override string ToString()
        {
            StringBuilder retString = new StringBuilder();

            for(int count = 0; count < Notes.Count; count++)
            {
                if (count > 0)
                {
                    retString.Append(" or ");
                }
                retString.Append(Notes[count]);
            }
            retString.Append("\t| line # " + LineNumber);
            retString.Append(" | " + LineText);
            return retString.ToString();
        }
    }
}
