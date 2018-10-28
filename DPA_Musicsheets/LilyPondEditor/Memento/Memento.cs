using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.LilyPondEditor.Memento
{
    class Memento
    {
        private Staff _lilypond;

        public Memento(Staff lilypond)
        {
            _lilypond = lilypond;
        }

        public Staff GetLilypond()
        {
            return _lilypond;
        }
    }
}
