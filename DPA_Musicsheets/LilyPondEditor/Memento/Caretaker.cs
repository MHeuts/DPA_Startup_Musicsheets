using System.Collections.Generic;
using System.Linq;

namespace DPA_Musicsheets.LilyPondEditor.Memento
{
    class Caretaker
    {
        private readonly Stack<Memento> _undo;
        private readonly Stack<Memento> _redo;

        public Caretaker()
        {
            _undo = new Stack<Memento>();
            _redo = new Stack<Memento>();
        }

        public void Save(string lilypond)
        {
            _undo.Push(new Memento(lilypond));
            _redo.Clear();
        }

        public string Undo(string lilypond)
        {
            _redo.Push(new Memento(lilypond));
            _undo.Pop();
            return _undo.Peek().GetLilypond();
        }

        public string Redo(string lilypond)
        {
            _undo.Push(new Memento(lilypond));
            return _redo.Pop().GetLilypond();
        }

        public bool CanRedo()
        {
            if (_redo.Count() <= 0)
                return false;
            return true;
        }

        public bool CanUndo()
        {
            if (_undo.Count() <= 1)
                return false;
            return true;
        }
    }
}
