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
    class StaffConverter : IValueConverter, IStaffElementVisitor
    {
        private ObservableCollection<MusicalSymbol> _symbols;

        public ObservableCollection<MusicalSymbol> Convert(Staff song)
        {
            return (ObservableCollection<MusicalSymbol>)Convert(song, typeof(ObservableCollection<MusicalSymbol>), null, null);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _symbols = new ObservableCollection<MusicalSymbol>();
            if (value is Staff staff)
            {
                staff.Accept(this);
            }
            return _symbols;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public void Visit(Staff staff)
        {
            var clef = new Clef(ClefType.GClef, 2);
            _symbols.Add(clef);
            _symbols.Add(new TimeSignature(TimeSignatureType.Numbers, (uint)staff.Rhythm.Item1, (uint)staff.Rhythm.Item2));
            foreach (var child in staff.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(Bar bar)
        {
            foreach (var musicNote in bar.MusicNotes)
            {
                if (musicNote.Tone == Tone.Silent)
                {
                    _symbols.Add(new Rest((MusicalSymbolDuration)musicNote.Duration));
                    continue;
                }
                musicNote.Duration = 1 / musicNote.Duration;
                var note = new Note(musicNote.Tone.ToString().ToUpper(), (int)musicNote.Modifier, musicNote.Octave, (MusicalSymbolDuration)musicNote.Duration, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });

                _symbols.Add(note);
            }
            _symbols.Add(new Barline());
        }
    }
}
