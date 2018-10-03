using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class EndOfTrackMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiConverterContext context, MetaMessage message)
        {
            // TODO: Figure out what to do here
            //throw new NotImplementedException();
        }
    }
}
