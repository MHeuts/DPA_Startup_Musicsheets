using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System.Linq;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class TempoMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiConverterContext context, MetaMessage message)
        {
            byte[] tempoBytes = message.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            var bpm = 60000000 / tempo;
            context.StaffBuilder.SetTempo(bpm);
        }
    }
}
