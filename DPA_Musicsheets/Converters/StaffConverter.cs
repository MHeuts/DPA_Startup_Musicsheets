using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DPA_Musicsheets.Models;
using PSAMControlLibrary;

namespace DPA_Musicsheets.Converters
{
    class StaffConverter : IValueConverter
    {
        public List<MusicalSymbol> Convert(Song song)
        {
            return (List<MusicalSymbol>)Convert(song, typeof(List<MusicalSymbol>), null, null);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Song = value as Song;
            if (Song == null) return null;
            
            var musicalSymbols = new List<MusicalSymbol>();

            var clef = new Clef(ClefType.GClef, 2);
            musicalSymbols.Add(clef);
            //TODO: REPETITON
            foreach (var bar in Song.Bars)
            {
                musicalSymbols.Add(new TimeSignature(TimeSignatureType.Numbers, (uint)bar.Rhythm.Item1, (uint)bar.Rhythm.Item2));
                foreach (var musicNote in bar.MusicNotes)
                {
                    if (musicNote.Tone == Tone.Silent)
                    {
                        musicalSymbols.Add(new Rest((MusicalSymbolDuration)musicNote.Duration));
                        continue;
                    }

                    var note = new Note(musicNote.Tone.ToString().ToUpper(), (int)musicNote.Modifier, musicNote.Octave, (MusicalSymbolDuration)musicNote.Duration, NoteStemDirection.Up, NoteTieType.None, null);

                    musicalSymbols.Add(note);
                }
                musicalSymbols.Add(new Barline());
            }
            return musicalSymbols;


            return musicalSymbols;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
