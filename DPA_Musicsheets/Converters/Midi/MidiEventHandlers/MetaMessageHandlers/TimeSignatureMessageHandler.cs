using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Linq;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class TimeSignatureMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiConverterContext context, MetaMessage message)
        {
            byte[] timeSignatureBytes = message.GetBytes();
            var rhythm = new Tuple<int, int>(timeSignatureBytes[0], (int)Math.Pow(2, timeSignatureBytes[1]));
            context.Rhythm = rhythm;
            context.StaffBuilder.SetRhythm(rhythm);
        }
    }
}
