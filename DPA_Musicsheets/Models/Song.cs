using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Song
    {
        public Song()
        {
            Bars = new List<Bar>();
        }

        public List<Bar> Bars { get; set; }
        public List<Repetition> Repetitions { get; set; }
    }
}
