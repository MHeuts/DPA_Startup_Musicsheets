using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.IO.FileHandlers
{
    public class MidiFileHandler : MusicFileHandler
    {
        private MidiConverter _converter;

        public MidiFileHandler(MidiConverter converter, MusicFileHandler next = null) : base(next)
        {
            _converter = converter;
        }

        public static List<string> GetExtensions() {
            return new List<string> { ".midi", ".mid"};
        }

        public static string GetFileType()
        {
            return "Midi";
        }

        public static string GetSupportedFileTypeString()
        {
            return MusicFileHandler.BuildSupportedFileTypeString(GetFileType(), GetExtensions());
        }

        public override List<string> Extensions => MidiFileHandler.GetExtensions();

        public override string FileType => GetFileType();

        public override string FileTypeString => GetSupportedFileTypeString();

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
