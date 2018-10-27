﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
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
                Octave = 0
            };

            foreach (char c in LilyNote)
            {
                if (c == '\'')
                    note.Octave++;
                else if (c == ',')
                    note.Octave--;
            }

            note.Tone = getNote(LilyNote[0]);
            note.Modifier = GetModifier(LilyNote.Substring(1, 2));
            note.Duration = GetDuration(LilyNote);
            if (LilyNote[LilyNote.Length] == '.')
                note.Dot = true;
           
            return note;
        }

        private double GetDuration(string lilyNote)
        {
            foreach (char c in lilyNote)
                if(char.IsDigit(c))
                    return 1/Convert.ToInt16(c);

            return 0;
        }

        public  MusicNote CreateRest()
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

        public Tone getNote(char tone)
        {
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
            switch (modifier)
            {
                case "fi":
                    return Modifier.Flat;
                case "gi":
                    return Modifier.Sharp;
            }

            return Modifier.None;
        }
    }
}
