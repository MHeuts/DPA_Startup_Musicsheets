using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DPA_Musicsheets.Converters.LilyPond
{
    public class LilyPondConverter : IValueConverter, IStaffElementVisitor
    {
        private StringBuilder LilyText;
        private Staff song;

        public string Convert(Staff song)
        {
            return (String)Convert(song, typeof(String), null, null);
        }

        public Staff ConvertBack(String lilyPond)
        {
            return (Staff)ConvertBack(lilyPond, typeof(Staff), null, null);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LilyText = new StringBuilder();
            var song = value as Staff;
            if (song != null)
            {
                // Visit
                song.Accept(this);
            }
            return LilyText.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lilyText = value as string;
            if (lilyText == null) return null;

            var song = new Staff();

            string[] lines = lilyText.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach(var line in lines)
            {
                line.Trim();
                if(line[0]!= '\\')
                {
                    song.Accept(Bar);
                }
            }
            

            return song;
            /*
            var content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");

            var tokens = new LinkedList<LilypondToken>();

            foreach (string s in content.Split(' ').Where(item => item.Length > 0))
            {
                LilypondToken token = new LilypondToken()
                {
                    Value = s
                };

                switch (s)
                {
                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                    case "\\repeat": token.TokenKind = LilypondTokenKind.Repeat; break;
                    case "\\alternative": token.TokenKind = LilypondTokenKind.Alternative; break;
                    case "{": token.TokenKind = LilypondTokenKind.SectionStart; break;
                    case "}": token.TokenKind = LilypondTokenKind.SectionEnd; break;
                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
                }

                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[~]?[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Note;
                }
                else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Rest;
                }

                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
            */
        }

        public void Visit(Staff staff)
        {
            LilyText.AppendLine("\\relative c' {");
            LilyText.AppendLine("\\clef treble");
            LilyText.AppendLine($"\\time {staff.Rhythm.Item1}/{staff.Rhythm.Item2}");
            LilyText.AppendLine($"\\tempo 4={staff.Bpm}");

            foreach (var child in staff.Children)
            {
                child.Accept(this);
            }

            LilyText.Append("}");
        }

        public void Visit(Bar bar)
        {
            foreach (var note in bar.MusicNotes)
            {
                if (note.Tone == Tone.Silent)
                    LilyText.Append("r");
                else
                    LilyText.Append(note.Tone);
                if (note.Modifier == Modifier.Sharp)
                    LilyText.Append("is");
                else if (note.Modifier == Modifier.Flat)
                    LilyText.Append("es");
                LilyText.Append($"{note.Duration}");
                if (note.Dot)
                    LilyText.Append(".");
                LilyText.Append(" ");
            }
            LilyText.Append("| \n");
        }
        /*
        private static LinkedList<LilypondToken> GetTokensFromLilypond(string content)
        {
            var tokens = new LinkedList<LilypondToken>();

            foreach (string s in content.Split(' ').Where(item => item.Length > 0))
            {
                LilypondToken token = new LilypondToken()
                {
                    Value = s
                };

                switch (s)
                {
                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                    case "\\repeat": token.TokenKind = LilypondTokenKind.Repeat; break;
                    case "\\alternative": token.TokenKind = LilypondTokenKind.Alternative; break;
                    case "{": token.TokenKind = LilypondTokenKind.SectionStart; break;
                    case "}": token.TokenKind = LilypondTokenKind.SectionEnd; break;
                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
                }

                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[~]?[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Note;
                }
                else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Rest;
                }

                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
        }
        */
        private Bar getBarFromLine(string line)
        {
            NoteFactory factory = new NoteFactory();
            Bar bar = new Bar();
            foreach (string s in line.Split(' ').Where(item => item.Length > 0))
            {
                bar.MusicNotes.Add(factory.Create(line));
            }
            return bar;
        }

        private static List<Bar> getBarListFromTokens(LinkedList<LilypondToken> tokens)
        {
            List<Bar> symbols = new List<Bar>();


            return symbols;
        }
        /*
        private static IEnumerable<MusicalSymbol> GetStaffsFromTokens(LinkedList<LilypondToken> tokens)
        {
            List<MusicalSymbol> symbols = new List<MusicalSymbol>();

            Clef currentClef = null;
            int previousOctave = 4;
            char previousNote = 'c';
            bool inRepeat = false;
            bool inAlternative = false;
            int alternativeRepeatNumber = 0;

            LilypondToken currentToken = tokens.First();
            while (currentToken != null)
            {
                // TODO: There are a lot of switches based on LilypondTokenKind, can't those be eliminated en delegated?
                // HINT: Command, Decorator, Factory etc.

                // TODO: Repeats are somewhat weirdly done. Can we replace this with the COMPOSITE pattern?
                switch (currentToken.TokenKind)
                {
                    case LilypondTokenKind.Unknown:
                        break;
                    case LilypondTokenKind.Repeat:
                        inRepeat = true;
                        symbols.Add(new Barline() { RepeatSign = RepeatSignType.Forward });
                        break;
                    case LilypondTokenKind.SectionEnd:
                        if (inRepeat && currentToken.NextToken?.TokenKind != LilypondTokenKind.Alternative)
                        {
                            inRepeat = false;
                            symbols.Add(new Barline() { RepeatSign = RepeatSignType.Backward, AlternateRepeatGroup = alternativeRepeatNumber });
                        }
                        else if (inAlternative && alternativeRepeatNumber == 1)
                        {
                            alternativeRepeatNumber++;
                            symbols.Add(new Barline() { RepeatSign = RepeatSignType.Backward, AlternateRepeatGroup = alternativeRepeatNumber });
                        }
                        else if (inAlternative && currentToken.NextToken.TokenKind == LilypondTokenKind.SectionEnd)
                        {
                            inAlternative = false;
                            alternativeRepeatNumber = 0;
                        }
                        break;
                    case LilypondTokenKind.SectionStart:
                        if (inAlternative && currentToken.PreviousToken.TokenKind != LilypondTokenKind.SectionEnd)
                        {
                            alternativeRepeatNumber++;
                            symbols.Add(new Barline() { AlternateRepeatGroup = alternativeRepeatNumber });
                        }
                        break;
                    case LilypondTokenKind.Alternative:
                        inAlternative = true;
                        inRepeat = false;
                        currentToken = currentToken.NextToken; // Skip the first bracket open.
                        break;
                    case LilypondTokenKind.Note:
                        // Tied
                        // TODO: A tie, like a dot and cross or mole are decorations on notes. Is the DECORATOR pattern of use here?
                        NoteTieType tie = NoteTieType.None;
                        if (currentToken.Value.StartsWith("~"))
                        {
                            tie = NoteTieType.Stop;
                            var lastNote = symbols.Last(s => s is Note) as Note;
                            if (lastNote != null) lastNote.TieType = NoteTieType.Start;
                            currentToken.Value = currentToken.Value.Substring(1);
                        }
                        // Length
                        int noteLength = Int32.Parse(Regex.Match(currentToken.Value, @"\d+").Value);
                        // Crosses and Moles
                        int alter = 0;
                        alter += Regex.Matches(currentToken.Value, "is").Count;
                        alter -= Regex.Matches(currentToken.Value, "es|as").Count;
                        // Octaves
                        int distanceWithPreviousNote = notesorder.IndexOf(currentToken.Value[0]) - notesorder.IndexOf(previousNote);
                        if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
                        {
                            distanceWithPreviousNote -= 7; // The number of notes in an octave
                        }
                        else if (distanceWithPreviousNote < -3)
                        {
                            distanceWithPreviousNote += 7; // The number of notes in an octave
                        }

                        if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) >= 7)
                        {
                            previousOctave++;
                        }
                        else if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) < 0)
                        {
                            previousOctave--;
                        }

                        // Force up or down.
                        previousOctave += currentToken.Value.Count(c => c == '\'');
                        previousOctave -= currentToken.Value.Count(c => c == ',');

                        previousNote = currentToken.Value[0];

                        var note = new Note(currentToken.Value[0].ToString().ToUpper(), alter, previousOctave, (MusicalSymbolDuration)noteLength, NoteStemDirection.Up, tie, new List<NoteBeamType>() { NoteBeamType.Single });
                        note.NumberOfDots += currentToken.Value.Count(c => c.Equals('.'));

                        symbols.Add(note);
                        break;
                    case LilypondTokenKind.Rest:
                        var restLength = Int32.Parse(currentToken.Value[1].ToString());
                        symbols.Add(new Rest((MusicalSymbolDuration)restLength));
                        break;
                    case LilypondTokenKind.Bar:
                        symbols.Add(new Barline() { AlternateRepeatGroup = alternativeRepeatNumber });
                        break;
                    case LilypondTokenKind.Clef:
                        currentToken = currentToken.NextToken;
                        if (currentToken.Value == "treble")
                            currentClef = new Clef(ClefType.GClef, 2);
                        else if (currentToken.Value == "bass")
                            currentClef = new Clef(ClefType.FClef, 4);
                        else
                            throw new NotSupportedException($"Clef {currentToken.Value} is not supported.");

                        symbols.Add(currentClef);
                        break;
                    case LilypondTokenKind.Time:
                        currentToken = currentToken.NextToken;
                        var times = currentToken.Value.Split('/');
                        symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));
                        break;
                    case LilypondTokenKind.Tempo:
                        // Tempo not supported
                        break;
                    default:
                        break;
                }
                currentToken = currentToken.NextToken;
            }

            return symbols;
        }

    */  
    }
}
