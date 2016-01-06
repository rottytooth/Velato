using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

namespace Rottytooth.Esolang.Velato
{
    public class MidiLoader
    {
        public const int DEFAULT_PITCH_VALUE = 8192;
        public const int MAX_PITCH_VALUE = 16383;

        public List<MidiEvent> Events { get; private set; }

        public List<Note> Notes { get; private set; }

        public bool IsLoaded { get; private set; }

        private double _smallestPitchCorrection { get; set; }

        public int SmallestInterval { get; private set; }

        private int currentPitch = DEFAULT_PITCH_VALUE;

        public MidiLoader()
        {
            Events = null;
            Notes = null;
            IsLoaded = false;
            _smallestPitchCorrection = 0.0;
        }

        public void Load(string path)
        {
            Notes = new List<Note>();
            MidiFile midi = new MidiFile(path);
            bool trackContainsNotes = false;

            for (int i = 0; i < midi.Tracks; i++) // does midi.Events always equal midi.Tracks ????
            {
                List<MidiEvent> currentTrack = new List<MidiEvent>();

                foreach (MidiEvent note in midi.Events[i])
                {
                    if (note.CommandCode == MidiCommandCode.PitchWheelChange)
                    {
                        // for microtonals
                        currentTrack.Add(note);
                        currentPitch = ((PitchWheelChangeEvent)note).Pitch;
                    }
                    if (note.CommandCode == MidiCommandCode.NoteOn)
                    {
                        bool isNoteOff = false;

                        // sometimes a NoteOn is actually a NoteOff
                        foreach (MidiEvent prevNoteOn in currentTrack)
                        {
                            if (prevNoteOn is NoteOnEvent && ((NoteOnEvent)prevNoteOn).OffEvent == note)
                            {
                                isNoteOff = true;
                            }
                        }
                        if (!isNoteOff)
                        {
                            currentTrack.Add((NoteOnEvent)note);
                            trackContainsNotes = true;

                            double pitchCorrection =
                                Math.Round(((double)currentPitch - DEFAULT_PITCH_VALUE) / (DEFAULT_PITCH_VALUE / 2.0), 4);

                            if (pitchCorrection > 0 &&
                                (Math.Abs(pitchCorrection) < _smallestPitchCorrection) || // assigned but new one is smaller
                                (_smallestPitchCorrection == 0)) // not assigned yet
                            {
                                _smallestPitchCorrection = pitchCorrection;
                            }

                            Notes.Add(new Note()
                            {
                                RawNumber = ((NoteOnEvent)note).NoteNumber,
                                Name = ((NoteOnEvent)note).NoteName,
                                Pitch =
                                    440.0 * 2.0 * (((NoteOnEvent)note).NoteNumber / 12.0) +
                                    (currentPitch - 8192.0) / (4096.0 * 12.0),
                                PitchCorrection = pitchCorrection
                            });
                        }
                    }
                }

                if (trackContainsNotes)
                {
                    SmallestInterval = (int)Math.Round(1.0 / _smallestPitchCorrection);

                    if (SmallestInterval > 10)
                        SmallestInterval = 16; // sixteenth tones is the smallest allowed
                    else if (SmallestInterval > 5)
                        SmallestInterval = 8; // eight tones
                    else if (SmallestInterval > 2)
                        SmallestInterval = 4; // quarter tones
                    else
                        SmallestInterval = 2; // default is half-steps

                    Events = currentTrack;
                    return;
                }
            }


            IsLoaded = true;
        }

    }
}
