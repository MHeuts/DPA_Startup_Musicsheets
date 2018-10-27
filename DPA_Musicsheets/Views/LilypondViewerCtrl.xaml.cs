using DPA_Musicsheets.LilyPondEditor.Shortcuts;
using DPA_Musicsheets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DPA_Musicsheets.Views
{
    /// <summary>
    /// Interaction logic for LilypondViewer.xaml
    /// </summary>
    /// 

    public interface ILilypondTextBox
    {
        void InsertAtCaretIndex(string text);
    }
    
    public partial class LilypondViewerCtrl : UserControl, ILilypondTextBox
    {
        private ShortcutListener shortcutListener;
        public LilypondViewerCtrl()
        {
            InitializeComponent();
            LilypondViewModel context = DataContext as LilypondViewModel;
            shortcutListener = context.ShortcutListener;
            context.TextBox = this;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            shortcutListener.Listen();
        }

        public void InsertAtCaretIndex(string text)
        {
            LilypondTextBox.Text = LilypondTextBox.Text.Insert(LilypondTextBox.CaretIndex, text);
        }
    }
}
