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
            // Make new staff if this is a time signature change
            if (context.Staff.Children.Count() > 0)
            {
                var staff = new Staff()
                {
                    Rhythm = context.Staff.Rhythm,
                    Bpm = context.Staff.Bpm,
                    Parent = context.Staff
                };
                context.Staff.Children.Add(staff);
                context.Staff = staff;
            }

            byte[] timeSignatureBytes = message.GetBytes();
            context.Staff.Rhythm = new Tuple<int, int>(timeSignatureBytes[0], (int)Math.Pow(2, timeSignatureBytes[1]));
        }
    }
}
