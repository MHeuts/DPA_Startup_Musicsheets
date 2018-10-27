using System;
using System.Windows.Input;

namespace DPA_Musicsheets.LilyPondEditor.Shortcuts
{
    public class ShortcutListener
    {
        private Shortcut shortcut;

        public void AddShortcut(Key[] keys, Action command)
        {
            if (shortcut != null)
            {
                var newShortcut = new Shortcut(keys, command, shortcut);
                shortcut = newShortcut;
            }
            else
                shortcut = new Shortcut(keys, command);
        }

        public void Listen()
        {
            if (shortcut != null)
                shortcut.Execute();
        }
    }
}
