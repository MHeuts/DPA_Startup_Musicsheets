using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public abstract class MetaMessageHandler : IMidiMessageHandler
    {
        public void HandleMessage(MidiConverterContext context, IMidiMessage message)
        {
            if (message is MetaMessage metaMessage)
            {
                HandleMetaMessage(context, metaMessage);
            }
        }

        protected abstract void HandleMetaMessage(MidiConverterContext context, MetaMessage message);
    }
}
