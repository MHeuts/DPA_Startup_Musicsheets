﻿using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DPA_Musicsheets.Converters.LilyPond
{
    public class LilyPondConverter : IValueConverter, IStaffElementVisitor
    {
        private StringBuilder LilyText;
        private NoteFactory _noteFactory;
        private StaffBuilder _staffBuilder;

        public LilyPondConverter(NoteFactory noteFactory)
        {
            _staffBuilder = new StaffBuilder();
            _noteFactory = noteFactory;
        }

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

            LinkedList<LilypondToken> lilypondTokens = GetTokensFromLilypond(lilyText);

            var currentToken = lilypondTokens.First();

            while (currentToken != null)
            {
                switch (currentToken.TokenKind)
                {
                    case LilypondTokenKind.Note:
                        _staffBuilder.AddNote(_noteFactory.Create(currentToken.Value));
                        break;
                    case LilypondTokenKind.Tempo:
                        var bpm = currentToken.NextToken.Value.Substring(currentToken.NextToken.Value.LastIndexOf('=') + 1);
                        _staffBuilder.SetTempo(int.Parse(bpm));
                        break;
                    case LilypondTokenKind.Time:
                        var times = currentToken.NextToken.Value.Split('/');
                        _staffBuilder.SetRhythm(new Tuple<int, int>(int.Parse(times[0]), int.Parse(times[1])));
                        break;
                    default:
                        break;
                }
                currentToken = currentToken.NextToken;
            }

            return _staffBuilder.Build();
        }

        public void Visit(Staff staff)
        {
            LilyText.AppendLine("\\relative c' { ");
            LilyText.AppendLine("\\clef treble ");
            LilyText.AppendLine($"\\time {staff.Rhythm.Item1}/{staff.Rhythm.Item2} ");
            LilyText.AppendLine($"\\tempo 4={staff.Bpm} ");

            foreach (var child in staff.Children)
            {
                child.Accept(this);
            }

            LilyText.Append("} ");
        }

        public void Visit(Bar bar)
        {
            foreach (var note in bar.MusicNotes)
            {
                if (note.Tone == Tone.Silent)
                    LilyText.Append("r");
                else
                    LilyText.Append(note.Tone);
                if (note.Octave == 5)
                    LilyText.Append('\'');
                else if (note.Octave == 3) 
                    LilyText.Append(',');
                if (note.Modifier == Modifier.Sharp)
                    LilyText.Append("is");
                else if (note.Modifier == Modifier.Flat)
                    LilyText.Append("es");
                LilyText.Append($"{1/note.Duration}");
                if (note.Dot)
                    LilyText.Append(".");
                LilyText.Append(" ");
            }
            LilyText.Append("| \n");
        }
        
        private static LinkedList<LilypondToken> GetTokensFromLilypond(string content)
        {
            var tokens = new LinkedList<LilypondToken>();

            foreach (string s in content.Split(' ').Where(item => item.Length > 0))
            {
                var tekst = s.Replace("\r\n", string.Empty);
                LilypondToken token = new LilypondToken()
                {
                    Value = tekst
                };

                switch (tekst)
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

                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[~]?[a-g][,'eis]*[0-9]+[.]*", RegexOptions.IgnoreCase).IsMatch(token.Value))
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
    }
}
