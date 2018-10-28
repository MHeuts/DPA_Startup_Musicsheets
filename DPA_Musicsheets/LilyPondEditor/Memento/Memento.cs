using DPA_Musicsheets.Models;

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
