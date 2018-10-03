using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.Midi.MidiEventHandlers.ChannelMessageHandlers
{
    public class NoteOnMessageHandler : ChannelMessageHandler
    {
        protected override void HandleChannelMessage(MidiConverterContext context, ChannelMessage message)
        {
            if (message.Data2 > 0) // Data2 = loudness
            {
                //create note with height
                context.CurrentNote = context.NoteFactory.Create(message.Data1);

                //previousMidiKey = channelMessage.Data1;
                context.StartedNoteIsClosed = false;
            }
            else if (!context.StartedNoteIsClosed)
            {
                // Finish the previous note with the length.
                context.CurrentNote.Duration = GetNoteLength(context.PreviousNoteAbsoluteTicks, context.MidiEvent.AbsoluteTicks, context.Sequence.Division, context.Bar.Rhythm.Item1, context.Bar.Rhythm.Item2, out bool dot, out double percentageOfBar);
                context.CurrentNote.Dot = dot;

                context.PercentageOfBarReached += percentageOfBar;
                context.PreviousNoteAbsoluteTicks = context.MidiEvent.AbsoluteTicks;

                context.StartedNoteIsClosed = true;

                context.Bar.MusicNotes.Add(context.CurrentNote);
                context.CurrentNote = null;

                if (context.PercentageOfBarReached >= 1)
                {
                    context.PercentageOfBarReached -= 1;
                    context.Song.Bars.Add(context.Bar);
                    context.Bar = new Bar {
                        Rhythm = context.Bar.Rhythm,
                        Bpm = context.Bar.Bpm
                    };
                }
            }
            else
            {
                // TODO: Rest length?
                var note = context.NoteFactory.CreateRest();
                context.Bar.MusicNotes.Add(note);
            }
        }

        private double GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar, out bool hasDot, out double percentageOfBar)
        {
            var lengths = new int[] { 1, 2, 4, 8, 16, 32 };

            percentageOfBar = 0;
            hasDot = false;

            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                return 0;
            }

            double percentageOfBeatNote = deltaTicks / division;
            percentageOfBar = (1.0 / beatsPerBar) * percentageOfBeatNote;

            if (!(percentageOfBeatNote == 4 || percentageOfBeatNote == 2 || percentageOfBeatNote == 1 || percentageOfBeatNote == 0.5 || percentageOfBeatNote == 0.25 || percentageOfBeatNote == 0.125))
            {
                hasDot = true;
                percentageOfBeatNote = percentageOfBeatNote / 3 * 2;
            }
            percentageOfBeatNote = percentageOfBeatNote / 4;

            return percentageOfBeatNote;
        }
    }

    
}
