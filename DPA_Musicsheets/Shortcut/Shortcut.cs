using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.LilyPondEditor.Shortcuts
{
    public class Shortcut
    {
        private Key[] keys;
        private Shortcut next;
        private Action command;

        public Shortcut(Key[] keys, Action command, Shortcut next = null)
        {
            this.keys = keys;
            this.command = command;
            this.next = next;
        }

        private bool correctShortcut()
        {
            foreach (var key in keys)
                if (!Keyboard.IsKeyDown(key))
                    return false;

            return true;
        }

        public void Execute()
        {
            if (correctShortcut())
                command();
            else if (next != null)
                next.Execute();
        }
    }
}
