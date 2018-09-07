using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class MusicNote
    {
        public ToneEnum Tone { get; set; }
        public int Octave { get; set; }
        public Char Modifier { get; set; }
        public int Duration { get; set; }
        public bool Dot { get; set; }
    }
}
