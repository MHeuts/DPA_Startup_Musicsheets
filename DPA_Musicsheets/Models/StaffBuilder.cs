using System;
using System.Linq;

namespace DPA_Musicsheets.Models
{
    public class StaffBuilder
    {
        private Staff _currentStaff;
        private Staff _rootStaff;
        private Bar _currentBar;
        private double _barProgression;

        public StaffBuilder()
        {
            Reset();
        }

        public void AddNote(Tone tone, int duration, int octave, Modifier modifier, bool dot)
        {
            var note = new MusicNote
            {
                Tone = tone,
                Octave = octave,
                Duration = duration,
                Modifier = modifier,
                Dot = dot
            };

            AddNote(note);
        }

        public void AddNote(MusicNote note)
        {
            _currentBar.MusicNotes.Add(note);
            if (_barProgression + note.TotalDuration >= _currentStaff.BarDuration)
            {
                CloseCurrentBar();
            }
            _barProgression = (_barProgression + note.TotalDuration) % _currentStaff.BarDuration;
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
            var staff = new Staff() { Bpm = _currentStaff.Bpm, Rhythm = _currentStaff.Rhythm, Parent = _currentStaff };
            _currentStaff.Children.Add(staff);
            _currentStaff = staff;
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
            _barProgression = 0;
        }

        public Staff Build()
        {
            var res = _rootStaff;
            Reset();
            return res;
        }


    }
}
