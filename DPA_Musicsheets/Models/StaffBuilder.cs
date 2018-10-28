using DPA_Musicsheets.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class StaffBuilder
    {
        private NoteFactory _noteFactory;
        private Staff _currentStaff;
        private Staff _rootStaff;
        private Bar _currentBar;

        public StaffBuilder(NoteFactory noteFactory)
        {
            _noteFactory = noteFactory;
            Reset();
        }

        public void AddNote(int key, int duration, int octave, Modifier modifier, bool dot)
        {
            var note = _noteFactory.Create(key);
            note.Octave = octave;
            note.Duration = duration;
            note.Modifier = modifier;
            note.Dot = dot;

            _currentBar.MusicNotes.Add(note);

            // Track progression into bar (loop around for progression into next bar)
            // If note.duration >= bar length remaining, add to bar, make new bar
        }

        public void SetRhythm(Tuple<int, int> rhythm)
        {
            if (_currentStaff.Rhythm != rhythm)
            {
                if (_currentStaff.Children.Count() > 0)
                {
                    StartNewStaff();
                }
                _currentStaff.Rhythm = rhythm;
            }
        }

        public void SetTempo(int bpm)
        {
            if (_currentStaff.Bpm != bpm)
            {
                if (_currentStaff.Children.Count() > 0)
                {
                    StartNewStaff();
                }
                _currentStaff.Bpm = bpm;
            }
        }

        public void StartNewStaff()
        {
            _currentStaff = new Staff() { Bpm = _currentStaff.Bpm, Rhythm = _currentStaff.Rhythm, Parent = _currentStaff };
        }

        public void CloseStaff()
        {
            // TODO: End bar?
            if (_currentStaff.Parent != null)
            {
                _currentStaff = _currentStaff.Parent;
            }
        }

        private void CloseCurrentBar()
        {
            _currentStaff.Children.Add(_currentBar);
            _currentBar = new Bar();
        }

        public void Reset()
        {
            _rootStaff = new Staff();
            _currentStaff = _rootStaff;
            _currentBar = new Bar();
        }

        public Staff Build()
        {
            var res = _rootStaff;
            Reset();
            return res;
        }


    }
}
