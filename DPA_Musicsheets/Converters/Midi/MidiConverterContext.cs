using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Midi
{
    public abstract class MidiConverterContext
    {
        protected MidiConverterContext(NoteFactory nf, MidiMessageHandlerFactory mmf)
        {
            NoteFactory = nf;
            MidiMessageHandlerFactory = mmf;
        }
        
        public Sequence Sequence { get; set; }
        public MidiEvent MidiEvent { get; set; }
        public int SequenceCount { get; set; }
        public double PercentageOfBarReached { get; set; }
        public int PreviousNoteAbsoluteTicks { get; set; }
        public bool StartedNoteIsClosed { get; set; }
        public Song Song { get; set; }
        public Bar Bar { get; set; }
        public MusicNote CurrentNote { get; set; }
        public NoteFactory NoteFactory { get; }
        public MidiMessageHandlerFactory MidiMessageHandlerFactory { get; }
        public Track Track { get; set; }
    }
}
