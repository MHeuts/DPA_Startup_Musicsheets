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
    public class MidiConverter : MidiConverterContext, IValueConverter
    {
        public MidiConverter(NoteFactory nf, MidiMessageHandlerFactory mmf):base(nf, mmf)
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

            var sequence = new Sequence();

            // TODO: Conversion

            return sequence;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Sequence = value as Sequence;
            if (Sequence == null) return null;

            SequenceCount = 0;
            Staff = new Staff();
            Bar = new Bar();

            for (; SequenceCount < Sequence.Count();)
            {
                Track = Sequence[SequenceCount++];
                foreach (var midiEvent in Track.Iterator())
                {
                    MidiEvent = midiEvent;
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    var handler = MidiMessageHandlerFactory.CreateForMessage(midiMessage);
                    handler?.HandleMessage(this, midiMessage);
                }
            }
            
            return Staff;
        }
    }
}
