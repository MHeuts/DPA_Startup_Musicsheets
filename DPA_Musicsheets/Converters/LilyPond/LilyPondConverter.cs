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
    class LilyPondConverter : IValueConverter
    {
        public string Convert(Song song)
        {
            return (String)Convert(song, typeof(String), null, null);
        }

        public Song ConvertBack(String lilyPond)
        {
            return (Song)ConvertBack(lilyPond, typeof(Song), null, null);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder LilyText = new StringBuilder();
            var song = value as Song;
            if (song != null)
            {
                LilyText.AppendLine("\\relative c' {");
                LilyText.AppendLine("\\clef treble");
                
                foreach (var bar in song.Bars)
                {
                    LilyText.AppendLine($"\\time {bar.Rhythm.Item1}/{bar.Rhythm.Item2}");
                    LilyText.AppendLine($"\\tempo 4={bar.Bpm}");
                    foreach(var note in bar.MusicNotes)
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
                        if(note.Dot)
                            LilyText.Append(".");
                        LilyText.Append(" ");
                    }
                    LilyText.Append("| \n");
                }

                LilyText.Append("}");

            }
            return LilyText.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
