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

        public AddNote(MusicNote note)
        {
            _bar.MusicNotes.Add(note);
        }
    }
}
