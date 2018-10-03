using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class TempoMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiConverterContext context, MetaMessage message)
        {
            byte[] tempoBytes = message.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            context.Bar.Bpm = 60000000 / tempo;
        }
    }
}
