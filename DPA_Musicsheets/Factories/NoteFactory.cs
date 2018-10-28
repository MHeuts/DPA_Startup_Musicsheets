using DPA_Musicsheets.Models;

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
    }
}
