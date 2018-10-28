using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;

namespace DPA_Musicsheets.IO.FileHandlers
{
    public class MidiFileHandler : MusicFileHandler
    {
        private MidiConverter _converter;

        public MidiFileHandler(MidiConverter converter, MusicFileHandler next = null) : base("Midi", next)
        {
            _converter = converter;
            Extensions.Add(".midi");
            Extensions.Add(".mid");
        }

        override protected Staff Load(string fileName)
        {
            var midiSequence = new Sequence();
            midiSequence.Load(fileName);

            return _converter.ConvertBack(midiSequence);
        }

        override protected bool Save(string fileName, Staff staff)
        {
            var sequence = _converter.Convert(staff);
            if (sequence == null) return false;
            try {
                sequence.Save(fileName);
            } catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
