using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.IO.Midi
{
    public class MidiFileHandler : IMusicFileHandler
    {
        public List<string> Extensions { get; private set; }

        public MidiFileHandler()
        {
            Extensions = new List<string> { "midi", "mid" };
        }

        public Song LoadFile(string fileName)
        {
            var converter = new MidiConverter(new NoteFactory(), new MidiMessageHandlerFactory());
            var midiSequence = new Sequence();
            midiSequence.Load(fileName);

            return converter.ConvertBack(midiSequence);
        }

        public void SaveFile(string fileName, Song song)
        {
            throw new NotImplementedException();
        }
    }
}
