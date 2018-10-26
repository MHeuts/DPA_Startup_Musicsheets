using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.LilyPondEditor.Command.FileCommand
{
    abstract public class FileCommand : BaseCommand
    {
        public MusicLoader loader;
        protected FileCommand()
        {
            this.loader = loader;
        }
 
        abstract public void Execute();
    }
}
