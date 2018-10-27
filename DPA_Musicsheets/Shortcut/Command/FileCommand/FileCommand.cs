using DPA_Musicsheets.Managers;

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
