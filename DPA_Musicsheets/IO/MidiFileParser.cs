using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.IO
{
    public class MidiFileParser : IMusicFileParser
    {
        public Song parseFile(string fileName)
        {
            var sequence = new Sequence();
            sequence.Load(fileName);

            SongBuilder sb = new SongBuilder();
            BarBuilder bb = new BarBuilder();

            StringBuilder lilypondContent = new StringBuilder();
            lilypondContent.AppendLine("\\relative c' {");
            lilypondContent.AppendLine("\\clef treble");

            int beatNote = 4;
            int beatsPerBar = 4;
            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;

            for (int i = 0; i < sequence.Count(); i++)
            {
                Track track = sequence[i];

                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    // TODO: Split this switch statements and create separate logic.
                    // We want to split this so that we can expand our functionality later with new keywords for example.
                    // Hint: Command pattern? Strategies? Factory method?
                    switch (midiMessage.MessageType)
                    {
                        case MessageType.Meta:
                            var metaMessage = midiMessage as MetaMessage;
                            switch (metaMessage.MetaType)
                            {
                                case MetaType.TimeSignature:
                                    byte[] timeSignatureBytes = metaMessage.GetBytes();
                                    beatNote = timeSignatureBytes[0];
                                    beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                                    bb.SetRhythm(new Tuple<int, int>(beatNote, beatsPerBar));
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    var bpm = 60000000 / tempo;
                                    bb.SetTempo(bpm);
                                    break;
                                case MetaType.EndOfTrack:
                                    if (previousNoteAbsoluteTicks > 0)
                                    {
                                        // Finish the last notelength.
                                        //double percentageOfBar;
                                        //lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
                                        //lilypondContent.Append(" ");

                                        //percentageOfBarReached += percentageOfBar;
                                        //if (percentageOfBarReached >= 1)
                                        //{
                                        //    lilypondContent.AppendLine("|");
                                        //    percentageOfBar = percentageOfBar - 1;
                                        //}
                                    }
                                    break;
                                default: break;
                            }
                            break;
                        case MessageType.Channel:
                            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                            if (channelMessage.Command == ChannelCommand.NoteOn)
                            {
                                if (channelMessage.Data2 > 0) // Data2 = loudness
                                {
                                    previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    double percentageOfBar;
                                    bool hasDot;
                                    //lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;

                                    //TODO: Notelength
                                    var toneInfo = GetTone(previousMidiKey);
                                    var noteLength = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, beatNote, beatsPerBar, out hasDot, out percentageOfBar);
                                    bb.AddNote(toneInfo.Item1, noteLength, hasDot, toneInfo.Item2, toneInfo.Item3);
                                    
                                    percentageOfBarReached += percentageOfBar;
                                    if (percentageOfBarReached >= 1)
                                    {
                                        sb.AddBar(bb.Build());
                                        percentageOfBarReached -= 1;
                                    }
                                    startedNoteIsClosed = true;
                                }
                                else
                                {
                                    lilypondContent.Append("r");
                                }
                            }
                            break;
                    }
                }
            }

            return sb.build();

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

        private Tuple<ToneEnum, int, char> GetTone(int midiKey)
        {
            var octave = 0;
            char modifier = '0';
            ToneEnum tone = ToneEnum.C;

            var val = new Tuple<ToneEnum, int, char>(ToneEnum.A, (midiKey / 12) - 1, '0');
            switch (midiKey % 12)
            {
                case 0:
                    tone = ToneEnum.C;
                    break;
                case 1:
                    tone = ToneEnum.C;
                    modifier = '#';
                    break;
                case 2:
                    tone = ToneEnum.D;
                    break;
                case 3:
                    tone = ToneEnum.D;
                    modifier = '#';
                    break;
                case 4:
                    tone = ToneEnum.E;
                    break;
                case 5:
                    tone = ToneEnum.F;
                    break;
                case 6:
                    tone = ToneEnum.F;
                    modifier = '#';
                    break;
                case 7:
                    tone = ToneEnum.G;
                    break;
                case 8:
                    tone = ToneEnum.G;
                    modifier = '#';
                    break;
                case 9:
                    tone = ToneEnum.A;
                    break;
                case 10:
                    tone = ToneEnum.A;
                    modifier = '#';
                    break;
                case 11:
                    tone = ToneEnum.B;
                    break;
            }

            return new Tuple<ToneEnum, int, char>(tone, octave, modifier);
        }
    }
}