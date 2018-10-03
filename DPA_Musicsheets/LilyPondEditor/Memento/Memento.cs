using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.LilyPondEditor.Memento
{
    class Memento
    {
        private string _lilypond;

        public Memento(string lilypond)
        {
            _lilypond = lilypond;
        }

        public string GetLilypond()
        {
            return _lilypond;
        }
    }
}
