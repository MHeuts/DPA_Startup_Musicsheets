using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class SongBuilder
    {
        private Song _song;

        SongBuilder()
        {
            _song = new Song();
        }

        public void AddBar(Bar bar) {
            _song.Bars.Add(bar);
        }

        public void AddRepetition(Repetition repetition)
        {
            _song.Repetitions.Add(repetition);
        }

        public Song build()
        {
            var ret = _song;
            _song = new Song();
            return ret;
        }
    }
}
