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
                context.CurrentNote.Duration = GetNoteLength(context.PreviousNoteAbsoluteTicks, context.MidiEvent.AbsoluteTicks, context.Sequence.Division, context.Rhythm.Item1, context.Rhythm.Item2, out bool dot);
                context.CurrentNote.Dot = dot;

                context.PreviousNoteAbsoluteTicks = context.MidiEvent.AbsoluteTicks;

                context.StartedNoteIsClosed = true;

                context.StaffBuilder.AddNote(context.CurrentNote);
                context.CurrentNote = null;
            }
            else
            {
                // TODO: Rest length?
                context.CurrentNote = context.NoteFactory.CreateRest();
                context.StartedNoteIsClosed = false;
                //context.Bar
                //context.Bar.MusicNotes.Add(note);
            }
        }

        private double GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar, out bool hasDot)
        {
            hasDot = false;

            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                return 0;
            }

            double percentageOfBeatNote = deltaTicks / division;

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
