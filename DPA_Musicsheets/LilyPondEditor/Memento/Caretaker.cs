using DPA_Musicsheets.Models;
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

        public void Save(Staff lilypond)
        {
            _undo.Push(new Memento(lilypond));
            _redo.Clear();
        }

        public Staff Undo(Staff lilypond)
        {
            _redo.Push(new Memento(lilypond));
            _undo.Pop();
            return _undo.Peek().GetLilypond();
        }

        public Staff Redo(Staff lilypond)
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
