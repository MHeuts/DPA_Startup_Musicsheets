using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public abstract class ChannelMessageHandler : IMidiMessageHandler
    {
        public void HandleMessage(MidiEventSequencerContext context, IMidiMessage message)
        {
            var channelMessage = message as ChannelMessage;
            if(channelMessage != null)
            {
                HandleChannelMessage(context, channelMessage);
            }
        }

        protected abstract void HandleChannelMessage(MidiEventSequencerContext context, ChannelMessage message);
    }
}
