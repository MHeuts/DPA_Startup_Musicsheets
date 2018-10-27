using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers
{
    public class TempoMessageHandler : MetaMessageHandler
    {
        protected override void HandleMetaMessage(MidiConverterContext context, MetaMessage message)
        {
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

            byte[] tempoBytes = message.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            context.Staff.Bpm = 60000000 / tempo;
        }
    }
}
