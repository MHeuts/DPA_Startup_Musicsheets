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
        protected override void HandleMetaMessage(MidiEventSequencerContext context, MetaMessage message)
        {
            byte[] tempoBytes = message.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);

            if (context.Staff.Children.Count() > 0)
            {
                var staff = new Staff { Bpm = context.Staff.Bpm, Rhythm = context.Staff.Rhythm };
                context.Staff.Children.Add(staff);
                context.Staff = staff;
            }

            context.Staff.Bpm = 60000000 / tempo;
        }
    }
}
