using DPA_Musicsheets.LilyPondEditor.Shortcuts;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        private readonly ShortcutListener _shortcutListener;
        private MusicManager _musicManager;

                public ShortcutListener ShortcutListener => _shortcutListener;

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        /// The current state can be used to display some text.
        /// "Rendering..." is a text that will be displayed for example.
        /// </summary>
        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        public Staff Song { get; private set; }

        public MainViewModel(MusicManager musicManager)
        {
            _musicManager = musicManager;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
            // Not a dependency as every VM needs it's own for context awareness
            _shortcutListener = new ShortcutListener();
            SetShortCuts();
        }

        private void SetShortCuts()
        {
            ShortcutListener.AddShortcut(new Key[] { Key.LeftCtrl, Key.O }, OpenFileCommand);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = _musicManager.GetSupportedFilesString() };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            _musicManager.LoadFromFile(FileName);
        });

        #region Focus and key commands, these can be used for implementing hotkeys
        //public ICommand OnLostFocusCommand => new RelayCommand(() =>
        //{
        //    Console.WriteLine("Maingrid Lost focus");
        //});

        //public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        //{
        //    ShortcutListener.Listen();
        //});

        //public ICommand OnKeyUpCommand => new RelayCommand(() =>
        //{
        //    Console.WriteLine("Key Up");
        //});

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            ViewModelLocator.Cleanup();
        });

        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
