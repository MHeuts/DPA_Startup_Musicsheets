using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;

namespace DPA_Musicsheets.Converters.Midi
{
    public abstract class MidiConverterContext
    {
        protected MidiConverterContext(NoteFactory nf, MidiMessageHandlerFactory mmf)
        {
            NoteFactory = nf;
            MidiMessageHandlerFactory = mmf;
            StaffBuilder = new StaffBuilder();
        }
        
        public StaffBuilder StaffBuilder { get; set; }
        public Sequence Sequence { get; set; }
        public MidiEvent MidiEvent { get; set; }
        public int SequenceCount { get; set; }
        public int PreviousNoteAbsoluteTicks { get; set; }
        public bool StartedNoteIsClosed { get; set; }
        public MusicNote CurrentNote { get; set; }
        public NoteFactory NoteFactory { get; }
        public MidiMessageHandlerFactory MidiMessageHandlerFactory { get; }
        public Track MetaTrack { get; set; }
        public Track InstrumentTrack { get; set; }
        public Staff Staff { get; set; }
        public Tuple<int, int> Rhythm { get; set; }
    }
}
