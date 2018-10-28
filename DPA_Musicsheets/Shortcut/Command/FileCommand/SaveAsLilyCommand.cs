using Microsoft.Win32;

namespace DPA_Musicsheets.LilyPondEditor.Command.FileCommand
{
    public class SaveAsLilyCommand : FileCommand
    {
        public SaveAsLilyCommand() : base()
        { }

        public override void Execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly" };

            base.loader.SaveToLilypond(saveFileDialog.FileName);
        }
    }
}
