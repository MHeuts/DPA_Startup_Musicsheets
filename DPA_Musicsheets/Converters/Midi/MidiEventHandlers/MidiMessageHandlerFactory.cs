using DPA_Musicsheets.Converters.Midi.MidiEventHandlers.ChannelMessageHandlers;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers.MetaMessageHandlers;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;

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
            if (metaMessage is MetaMessage message && metaHandlers.ContainsKey(message.MetaType))
            {
                return metaHandlers[message.MetaType];
            }
            return null;
        }

        private IMidiMessageHandler CreateChannelMessageHandler(IMidiMessage channelMessage)
        {
            if (channelMessage is ChannelMessage message && channelHandlers.ContainsKey(message.Command))
            {
                return channelHandlers[message.Command];
            }
            return null;
        }
    }
}
