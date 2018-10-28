using DPA_Musicsheets.Models;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.Factories
{
    public class NoteFactory
    {
        public MusicNote Create(int midikey)
        {
            MusicNote note = new MusicNote
            {
                Octave = (midikey / 12) - 1
            };
            note.Tone = getNote(midikey, out Modifier modifier);
            note.Modifier = modifier;

            return note;
        }

        public MusicNote Create(string LilyNote)
        {
            MusicNote note = new MusicNote
            {
                Octave = 4
            };

            foreach (char c in LilyNote)
            {
                if (c == '\'')
                    note.Octave++;
                else if (c == ',')
                    note.Octave--;
            }

            note.Tone = getNote(LilyNote[0]);
            note.Modifier = GetModifier(LilyNote);
            note.Duration = GetDuration(LilyNote);
            if (LilyNote[LilyNote.Length - 1] == '.')
                note.Dot = true;

            return note;
        }


        public MusicNote CreateRest()
        {
            MusicNote note = new MusicNote
            {
                Tone = Tone.Silent,
            };
            return note;
        }

        public Tone getNote(int midiKey, out Modifier modifier)
        {
            modifier = Modifier.None;
            switch (midiKey % 12)
            {
                case 0:
                    return Tone.C;
                case 1:
                    modifier = Modifier.Sharp;
                    return Tone.C;
                case 2:
                    return Tone.D;
                case 3:
                    modifier = Modifier.Sharp;
                    return Tone.D;
                case 4:
                    return Tone.E;
                case 5:
                    return Tone.F;
                case 6:
                    modifier = Modifier.Sharp;
                    return Tone.F;
                case 7:
                    return Tone.G;
                case 8:
                    modifier = Modifier.Sharp;
                    return Tone.G;
                case 9:
                    return Tone.A;
                case 10:
                    modifier = Modifier.Sharp;
                    return Tone.A;
                case 11:
                    return Tone.B;
            }
            return Tone.Silent;
        }

        private double GetDuration(string lilyNote)
        {
            var number = Regex.Match(lilyNote, @"\d+").Value;

            return 1 / double.Parse(number);
        }

        public Tone getNote(char tone)
        {
            tone = char.ToLower(tone);
            switch (tone)
            {
                case 'a':
                    return Tone.A;
                case 'b':
                    return Tone.B;
                case 'c':
                    return Tone.C;
                case 'd':
                    return Tone.D;
                case 'e':
                    return Tone.E;
                case 'f':
                    return Tone.F;
                case 'g':
                    return Tone.G;
            }
            return Tone.Silent;
        }

        public Modifier GetModifier(string modifier)
        {
            if (modifier.Contains("fi"))
                return Modifier.Flat;
            else if (modifier.Contains("gi"))
                return Modifier.Sharp;

            return Modifier.None;
        }

    }
}
