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
    public class MidiEventSequencer : MidiEventSequencerContext
    {

        MidiEventSequencer(NoteFactory nf, MidiMessageHandlerFactory mmf) : base(nf, mmf)
        {
        }

        public Staff RunSequence(Sequence sequence)
        {
            Staff = new Staff();
            Bar = new Bar();

            

            for (var i = 0; i < sequence.Count(); i++)
            {
                foreach (var midiEvent in sequence[i].Iterator())
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
