using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public abstract class MetaMessageHandler : IMidiMessageHandler
    {
        public void HandleMessage(MidiConverterContext context, IMidiMessage message)
        {
            var metaMessage = message as MetaMessage;
            if(metaMessage != null)
            {
                HandleMetaMessage(context, metaMessage);
            }
        }

        protected abstract void HandleMetaMessage(MidiConverterContext context, MetaMessage message);
    }
}
