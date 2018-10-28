using Microsoft.Win32;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using DPA_Musicsheets;
using PSAMWPFControlLibrary;
using DPA_Musicsheets.ViewModels;
using DPA_Musicsheets.LilyPondEditor.Shortcuts;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShortcutListener _shortcutListener;

        public MainWindow()
        {
            InitializeComponent();
            _shortcutListener = (DataContext as MainViewModel).ShortcutListener;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            e.Handled = _shortcutListener.Listen();
        }
    }
}
