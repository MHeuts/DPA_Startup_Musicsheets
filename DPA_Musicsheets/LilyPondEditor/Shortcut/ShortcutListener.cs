using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.LilyPondEditor.Shortcuts
{
    class ShortcutListener
    {
        private Dictionary<Shortcut, BaseCommand> shortcuts;
        public ShortcutListener()
        {
            shortcuts = new Dictionary<Shortcut, BaseCommand>();
        }

        public void AddShortcut(Key[] keys, BaseCommand command)
        {
            shortcuts[new Shortcut(keys)] = command;
        }

    }
}
