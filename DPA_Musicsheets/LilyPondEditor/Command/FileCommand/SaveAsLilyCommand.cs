using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.LilyPondEditor.Command.FileCommand
{
    public class SaveAsLilyCommand : FileCommand
    {
        public SaveAsLilyCommand(MusicLoader loader) : base(loader)
        { }

        public override void Execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly" };

            base.loader.SaveToLilypond(saveFileDialog.FileName);
        }
    }
}
