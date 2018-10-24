using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.LilyPondEditor.Command.InsertCommand
{
    public class InsertSpeedCommand : InsertCommand
    {
        public InsertSpeedCommand(TextBox textBox) : base(textBox)
        { }

        public override void Execute()
        {
            base.Insert("\\tempo 4=120");
        }
    }
}
