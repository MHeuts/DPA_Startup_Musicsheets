using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class TimeSignatureMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiEventSequencerContext context, MetaMessage message)
        {
            byte[] timeSignatureBytes = message.GetBytes();

            if (context.Staff.Children.Count() > 0)
            {
                var staff = new Staff { Bpm = context.Staff.Bpm, Rhythm = context.Staff.Rhythm };
                context.Staff.Children.Add(staff);
                context.Staff = staff;
            }

            context.Staff.Rhythm = new Tuple<int, int>(timeSignatureBytes[0], (int)Math.Pow(2,timeSignatureBytes[1]));
        }
    }
}
