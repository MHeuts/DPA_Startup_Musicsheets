using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<MusicalSymbol> Convert(Staff song)
        {
            return (ObservableCollection<MusicalSymbol>)Convert(song, typeof(ObservableCollection<MusicalSymbol>), null, null);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var musicalSymbols = new ObservableCollection<MusicalSymbol>();
            var Song = value as Staff;
            if (Song != null)
            {

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
                    musicNote.Duration = 1 / musicNote.Duration;
                    var note = new Note(musicNote.Tone.ToString().ToUpper(), (int)musicNote.Modifier, musicNote.Octave, (MusicalSymbolDuration)musicNote.Duration, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });

                        musicalSymbols.Add(note);
                    }
                    musicalSymbols.Add(new Barline());

                }
            }
            return musicalSymbols;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
