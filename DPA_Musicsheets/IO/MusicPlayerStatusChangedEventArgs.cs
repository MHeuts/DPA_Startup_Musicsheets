using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.IO
{
    public class MusicPlayerStatusChangedEventArgs : EventArgs
    {
        private readonly bool _running;

        public MusicPlayerStatusChangedEventArgs(bool running)
        {
            _running = running;
        }

        public bool Running => _running;
    }
}
