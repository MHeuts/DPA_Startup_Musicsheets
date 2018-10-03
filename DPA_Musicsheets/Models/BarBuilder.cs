using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class BarBuilder
    {
        private Bar _bar;
        private Tuple<int, int> _rhythm;
        private int _tempo;

        public Tuple<int, int> Rhythm
        {
            get { return _rhythm; }
            set
            {
                _bar.Rhythm = value;
                _rhythm = value;
            }
        }

        public int Tempo
        {
            get { return _tempo; }
            set
            {
                _bar.Bpm = value;
                _tempo = value;
            }
        }

        public BarBuilder()
        {
            _bar = new Bar {
                Rhythm = _rhythm,
                Bpm = _tempo
            };
        }

        public void AddNote(MusicNote note)
        {
            _bar.MusicNotes.Add(note);
        }

        public void AddNote(Tone tone, double duration, bool dot, int octave, Modifier modifier)
        {
            _bar.MusicNotes.Add(new MusicNote {
                Tone = tone,
                Dot = dot,
                Duration = duration,
                Modifier = modifier,
                Octave = octave
            });
        }

        public Bar Build()
        {
            var copy = _bar;
            _bar = new Bar();
            return copy;
        }
    }
}
