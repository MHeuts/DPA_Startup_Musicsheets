using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Sanford.Multimedia.Midi;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;

namespace DPA_Musicsheets.Converters.Midi
{
    public class MidiConverter : MidiConverterContext, IValueConverter, IStaffElementVisitor
    {
        public MidiConverter(NoteFactory nf, MidiMessageHandlerFactory mmf) : base(nf, mmf)
        {

        }

        public Staff ConvertBack(Sequence sequence)
        {
            return (Staff)ConvertBack(sequence, typeof(Staff), null, null);
        }

        public Sequence Convert(Staff song)
        {
            return (Sequence)Convert(song, typeof(Sequence), null, null);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var song = value as Staff;
            if (song == null) return null;

            Sequence = new Sequence();
            PreviousNoteAbsoluteTicks = 0;
            MetaTrack = new Track();
            InstrumentTrack = new Track();
            Sequence.Add(MetaTrack);
            Sequence.Add(InstrumentTrack);

            song.Accept(this);

            MetaTrack.Insert(PreviousNoteAbsoluteTicks, MetaMessage.EndOfTrackMessage);
            InstrumentTrack.Insert(PreviousNoteAbsoluteTicks, MetaMessage.EndOfTrackMessage);

            return Sequence;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Sequence = value as Sequence;
            if (Sequence == null || Sequence.Count() < 2) return null;

            Staff = new Staff();
            Bar = new Bar();

            // Single instrument support only
            var track = Sequence[0];
            track.Merge(Sequence[1]);

            foreach (var midiEvent in track.Iterator())
            {
                MidiEvent = midiEvent;
                IMidiMessage midiMessage = midiEvent.MidiMessage;
                var handler = MidiMessageHandlerFactory.CreateForMessage(midiMessage);
                handler?.HandleMessage(this, midiMessage);
            }

            return Staff;
        }

        public void Visit(Staff staff)
        {
            InsertTempo(staff.Bpm);
            InsertTimeSignature(staff.Rhythm);
            Staff = staff;

            foreach (var child in staff.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(Bar bar)
        {
            foreach(var note in bar.MusicNotes)
            {
                // Calculate duration
                double absoluteLength = 1.0 / (1.0 / note.Duration);
                if (note.Dot)
                {
                    absoluteLength += (absoluteLength / 2.0);
                }

                double relationToQuartNote = Staff.Rhythm.Item1 / 4.0;
                double percentageOfBeatNote = (1.0 / Staff.Rhythm.Item1) / absoluteLength;
                double deltaTicks = (Sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                // TODO: have this work with different Clef
                List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
                // Calculate height
                int noteHeight = notesOrderWithCrosses.IndexOf(note.Tone.ToString().ToLower()) + ((note.Octave + 1) * 12);
                noteHeight += (int)note.Modifier;
                InstrumentTrack.Insert(PreviousNoteAbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                PreviousNoteAbsoluteTicks += (int)deltaTicks;
                InstrumentTrack.Insert(PreviousNoteAbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume
            }
        }

        private void InsertTempo(int bpm)
        {
            // Calculate tempo
            int speed = (60000000 / bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            MetaTrack.Insert(PreviousNoteAbsoluteTicks, new MetaMessage(MetaType.Tempo, tempo));
        }

        private void InsertTimeSignature(Tuple<int, int> rhythm)
        {
            byte[] timeSignature = new byte[4];
            timeSignature[0] = (byte)rhythm.Item1;
            timeSignature[1] = (byte)Math.Pow(rhythm.Item2, -2);
            MetaTrack.Insert(PreviousNoteAbsoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
        }
    }
}
