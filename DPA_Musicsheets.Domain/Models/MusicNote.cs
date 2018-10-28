using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Domain
{
    public class MusicNote
    {
        public Tone Tone { get; set; }
        public int Octave { get; set; }
        public Modifier Modifier { get; set; }
        public double Duration { get; set; }
        public bool Dot { get; set; }
    }
}
