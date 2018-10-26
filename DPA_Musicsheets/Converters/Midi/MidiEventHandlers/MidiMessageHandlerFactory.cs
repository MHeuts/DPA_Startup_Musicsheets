using DPA_Musicsheets.Converters.Midi.MidiEventHandlers.ChannelMessageHandlers;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers
{
    public class MidiMessageHandlerFactory
    {
        private Dictionary<MetaType,IMidiMessageHandler> metaHandlers;
        private Dictionary<ChannelCommand, IMidiMessageHandler> channelHandlers;

        public MidiMessageHandlerFactory()
        {
            // Meta
            metaHandlers = new Dictionary<MetaType, IMidiMessageHandler>();
            metaHandlers.Add(MetaType.TimeSignature, new TimeSignatureMessageHandler());
            metaHandlers.Add(MetaType.Tempo, new TempoMessageHandler());
            metaHandlers.Add(MetaType.EndOfTrack, new EndOfTrackMessageHandler());

            // Channel
            channelHandlers = new Dictionary<ChannelCommand, IMidiMessageHandler>();
            channelHandlers.Add(ChannelCommand.NoteOn, new NoteOnMessageHandler());
        }

        public IMidiMessageHandler CreateForMessage(IMidiMessage message) {
            if (message.MessageType == MessageType.Meta)
            {
                return CreateMetaMessageHandler(message);
            }
            if (message.MessageType == MessageType.Channel)
            {
                return CreateChannelMessageHandler(message);
            }
            return null;
        }

        private IMidiMessageHandler CreateMetaMessageHandler(IMidiMessage metaMessage)
        {
            var message = metaMessage as MetaMessage;
            Console.WriteLine($"{message.MessageType}; {message.MetaType}; {message.ToString()}");
            if (message != null && metaHandlers.ContainsKey(message.MetaType))
            {
                return metaHandlers[message.MetaType];
            }
            return null;
        }

        private IMidiMessageHandler CreateChannelMessageHandler(IMidiMessage channelMessage)
        {
            var message = channelMessage as ChannelMessage;
            if (message != null && channelHandlers.ContainsKey(message.Command))
            {
                return channelHandlers[message.Command];
            }
            return null;
        }
    }
}
