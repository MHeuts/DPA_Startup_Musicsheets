using System;
using System.Windows.Input;

namespace DPA_Musicsheets.LilyPondEditor.Shortcuts
{
    public class Shortcut
    {
        private ICommand Command { get; set; }
        private Key[] Keys { get; set; }
        private Shortcut Next { get; set; }

        public Shortcut(Key[] keys, ICommand command, Shortcut next = null)
        {
            this.Keys = keys;
            this.Command = command;
            this.Next = next;
        }

        private bool HandlesShortcut()
        {
            foreach (var key in Keys)
                if (!Keyboard.IsKeyDown(key))
                    return false;

            return true;
        }

        public bool Execute()
        {
            if (HandlesShortcut() && Command.CanExecute(null))
            {
                Command.Execute(null);
                return true;
            }
            else if (Next != null)
                return Next.Execute();
            return false;
        }
    }
}
