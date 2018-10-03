using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public interface IMidiMessageHandler
    {
        void HandleMessage(MidiConverterContext context, IMidiMessage message);
    }
}
