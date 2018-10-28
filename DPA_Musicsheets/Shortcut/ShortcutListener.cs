using System;
using System.Windows.Input;

namespace DPA_Musicsheets.LilyPondEditor.Shortcuts
{
    public class ShortcutListener
    {
        public Shortcut Shortcut { get; private set; }

        public void AddShortcut(Key[] keys, ICommand command)
        {
            if (Shortcut != null)
            {
                var newShortcut = new Shortcut(keys, command, Shortcut);
                Shortcut = newShortcut;
            }
            else
                Shortcut = new Shortcut(keys, command);
        }

        public bool Listen()
        {
            if (Shortcut != null)
                return Shortcut.Execute();
            return false;
        }
    }
}
