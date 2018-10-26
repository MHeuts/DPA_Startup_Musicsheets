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
    public class MidiConverter : IValueConverter, IStaffElementVisitor
    {
        private MidiEventSequencer _sequencer;

        public MidiConverter(MidiEventSequencer sequencer):base()
        {
            _sequencer = sequencer;
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

            //Track.Insert(PreviousNoteAbsoluteTicks, MetaMessage.EndOfTrackMessage);
            //// TODO: Conversion

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sequence = value as Sequence;
            if (sequence == null) return null;

            var staff = _sequencer.RunSequence(sequence);
            
            return staff;
        }

        public void Visit(Staff staff)
        {
            //InsertTempo(staff.Bpm);
            //InsertTimeSignature(staff.Rhythm);

            foreach(var child in staff.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(Bar bar)
        {

        }

        //private void InsertTempo(int bpm)
        //{
        //    // Calculate tempo
        //    int speed = (60000000 / bpm);
        //    byte[] tempo = new byte[3];
        //    tempo[0] = (byte)((speed >> 16) & 0xff);
        //    tempo[1] = (byte)((speed >> 8) & 0xff);
        //    tempo[2] = (byte)(speed & 0xff);
        //    Track.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));
        //}

        //private void InsertTimeSignature(Tuple<int, int> rhythm)
        //{
        //    byte[] timeSignature = new byte[4];
        //    timeSignature[0] = (byte)rhythm.Item1;
        //    timeSignature[1] = (byte)Math.Pow(rhythm.Item2, -2);
        //    Track.Insert(PreviousNoteAbsoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
        //}
    }
}
