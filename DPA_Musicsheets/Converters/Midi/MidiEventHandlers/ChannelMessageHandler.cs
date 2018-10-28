using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public abstract class ChannelMessageHandler : IMidiMessageHandler
    {
        public void HandleMessage(MidiConverterContext context, IMidiMessage message)
        {
            if (message is ChannelMessage channelMessage)
            {
                HandleChannelMessage(context, channelMessage);
            }
        }

        protected abstract void HandleChannelMessage(MidiConverterContext context, ChannelMessage message);
    }
}
