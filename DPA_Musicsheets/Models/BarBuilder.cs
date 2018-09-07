using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class BarBuilder
    {
        private Bar _bar;

        public BarBuilder()
        {
            _bar = new Bar();
        }

        public void AddNote(MusicNote note)
        {
            _bar.MusicNotes.Add(note);
        }

        public void AddNote(char tone, int duration, bool dot, int octave, char modifier)
        {
            _bar.MusicNotes.Add(new MusicNote {
                Tone = tone,
                Dot = dot,
                Duration = duration,
                Modifier = modifier,
                Octave = octave
            });
        }
    }
}
