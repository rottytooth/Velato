using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rottytooth.Esolang.Velato.Converter.Model;

namespace Rottytooth.Esolang.Velato.Converter
{
    public static class NoteUtil
    {
        public static NoteName[] CircleOfSharps = 
        {
            NoteName.F, // -1
            NoteName.C, // 0
            NoteName.G, // 1
            NoteName.D, // 2
            NoteName.A, // 3
            NoteName.E, // 4
            NoteName.B, // 5
        };

        // Both the following methods could be derived from the half step map above, but this makes it a little more clear and avoids looping
        // Someone smarter than me can take a stab at it
        public static int IntervalToWholeSteps(Interval interval)
        {
            switch(interval)
            {
                case Interval.Unison:
                    return 0;
                case Interval.MinorSecond:
                case Interval.MajorSecond:
                    return 1;
                case Interval.MinorThird:
                case Interval.MajorThird:
                    return 2;
                case Interval.PerfectFourth:
                    return 3;
                case Interval.DiminishedFifth:
                case Interval.PerfectFifth:
                    return 4;
                case Interval.MinorSixth:
                case Interval.MajorSixth:
                    return 5;
                case Interval.MinorSeventh:
                case Interval.MajorSeventh:
                default:
                    return 6;
            }
        }

        public static int GetAdjustment(Interval interval)
        {
            switch (interval)
            {
                case Interval.Unison:
                case Interval.MajorSecond:
                case Interval.MajorThird:
                case Interval.PerfectFourth:
                case Interval.PerfectFifth:
                case Interval.MajorSixth:
                case Interval.MajorSeventh:
                    return 0;
                case Interval.MinorSecond:
                case Interval.MinorThird:
                case Interval.DiminishedFifth:
                case Interval.MinorSixth:
                case Interval.MinorSeventh:
                default:
                    return -1;
            }
        }

        public static Note GetNoteByInterval(Note root, Interval interval)
        {
            NoteName intervalNote = (NoteName)((int)(root.Name + IntervalToWholeSteps(interval)) % Enum.GetNames(typeof(NoteName)).Length);
            Accidental accidental = Accidental.Natural;

            // positive = # of sharps, negative = # of flats
            int numberOfSharps = (int)root.Accidental * CircleOfSharps.Length;

            int keyCount = 0;
            for( ; CircleOfSharps[keyCount] != root.Name; keyCount++)
            {
            }
            numberOfSharps += Array.FindIndex(CircleOfSharps, w => w == root.Name) - 1; // -1 to make F -1, C 0, etc

            if (numberOfSharps > 0)
            {
                for(int sharpCount = 0; sharpCount < numberOfSharps; sharpCount++)
                {
                    if (CircleOfSharps[sharpCount % CircleOfSharps.Length] == intervalNote)
                        accidental++;
                }
            }
            else if (numberOfSharps < 0)
            {
                for (int flatCount = 0; flatCount > numberOfSharps; flatCount--)
                {
                    if (CircleOfSharps[CircleOfSharps.Length - 1 + (flatCount % CircleOfSharps.Length)] == intervalNote)
                        accidental--;
                }
            }

            // now offset the minor intervals
            accidental += GetAdjustment(interval);

            return new Note()
            {
                Accidental = accidental,
                Name = intervalNote
            };
        }
    }
}
