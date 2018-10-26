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
    class LilyPondConverter : IValueConverter, IStaffElementVisitor
    {
        private StringBuilder LilyText;

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
            throw new NotImplementedException();
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
    }
}
