using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.IO
{
    public class SongLoadedEventsArgs : EventArgs
    {
        public Song Song { get; set; }
    }
}
