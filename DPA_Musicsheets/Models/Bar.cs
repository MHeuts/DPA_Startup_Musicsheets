using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class Bar
    {
        public List<Note> MusicNotes { get; set; }
        public Tuple<int, int> Rhythm { get; set; }
        public int Bpm { get; set; }
    }
}
