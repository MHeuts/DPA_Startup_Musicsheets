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

        public void SetRhythm(Tuple<int, int> rhythm)
        {
            _rhythm = rhythm;
            _bar.Rhythm = rhythm;
        }


        public void SetTempo(int tempo)
        {
            _tempo = tempo;
            _bar.Bpm = tempo;
        }

        public void AddNote(ToneEnum tone, double duration, bool dot, int octave, char modifier)
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
