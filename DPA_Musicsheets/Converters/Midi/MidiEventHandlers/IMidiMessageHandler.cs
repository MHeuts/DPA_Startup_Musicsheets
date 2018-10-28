using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public interface IMidiMessageHandler
    {
        void HandleMessage(MidiConverterContext context, IMidiMessage message);
    }
}
