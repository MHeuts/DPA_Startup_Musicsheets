using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class Repetition
    {
        public MusicNote Start { get; set; }
        public MusicNote End { get; set; }
        public MusicNote Alternative { get; set; }
    }
}
