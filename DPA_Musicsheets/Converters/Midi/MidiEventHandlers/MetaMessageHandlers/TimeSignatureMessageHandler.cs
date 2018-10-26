using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class TimeSignatureMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiConverterContext context, MetaMessage message)
        {
            byte[] timeSignatureBytes = message.GetBytes();
            context.Staff.Rhythm = new Tuple<int, int>(timeSignatureBytes[0], (int)Math.Pow(2,timeSignatureBytes[1]));
        }
    }
}
