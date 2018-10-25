using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.LilyPondEditor.Command.InsertCommand
{
    public class InsertCleffCommand : InsertCommand
    {
        public InsertCleffCommand() : base()
        { }

        public override void Execute()
        {
            base.Insert("\\clef treble");
        }
    }
}
