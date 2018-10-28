using DPA_Musicsheets.LilyPondEditor.Shortcuts;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Text;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
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

        public Staff Song { get; set; }
        private ShortcutListener shortcutListener;

        //private MusicLoader _musicLoader;
        //private MidiFileParser _midiFileParser;
        private MusicManager _musicManager;

        public MainViewModel(MusicManager musicManager)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer?
            _musicManager = musicManager;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            // Get supported extensions
            var builder = new StringBuilder();
            foreach(var extension in _musicManager.GetSupportedExtensions())
            {
                builder.Append($"*{extension};");
            }
            var supported = builder.ToString();

            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = $"Supported files | {supported}" };
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
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
        });

        public ICommand OnKeyUpCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Key Up");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            ViewModelLocator.Cleanup();
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
