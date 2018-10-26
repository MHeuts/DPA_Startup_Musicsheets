using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.LilyPondEditor.Shortcuts
{
    class Shortcut
    {
        private Key[] keys;

        public Shortcut(Key[] keys)
        {
            this.keys = keys;
        }
    }
}
