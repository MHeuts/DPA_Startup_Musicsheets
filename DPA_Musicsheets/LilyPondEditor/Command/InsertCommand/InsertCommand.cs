using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.LilyPondEditor.Command.InsertCommand
{
    abstract public class InsertCommand : BaseCommand
    {
        public TextBox textBox;

        protected InsertCommand(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public abstract void Execute();

        public void Insert(string text)
        {
            var selectionIndex = textBox.SelectionStart;
            textBox.Text = textBox.Text.Insert(selectionIndex, text);
            textBox.SelectionStart = selectionIndex + text.Length;
        }

    }
}
