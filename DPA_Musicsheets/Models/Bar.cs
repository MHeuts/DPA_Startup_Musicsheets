using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Bar
    {
        public List<MusicNote> MusicNotes { get; set; }
        public Tuple<int, int> Rhythm { get; set; }
        public int Bpm { get; set; }
    }
}
