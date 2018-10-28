using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.Events;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.LilyPondEditor.Memento;
using DPA_Musicsheets.LilyPondEditor.Shortcuts;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;
        
        private MainViewModel _mainViewModel { get; set; }
        private LilyPondConverter _converter;

        private Caretaker _caretaker;
        private readonly MusicManager _musicManager;

        private Staff _song;
        public Staff Song
        {
            get { return _song; }
            set
            {
                _song = value;
                RaisePropertyChanged("Song");
            }
        }

        public ShortcutListener ShortcutListener { get; }
        public ILilypondTextBox TextBox { get; set; }
        private string _text;
        private string _previousText;
        private string _nextText;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private bool _textChangedByCommand = false;

        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;

        public LilypondViewModel(MusicManager musicManager, MainViewModel mainViewModel)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer and viewmodels don't know each other?
            // And viewmodels don't 
            _musicManager = musicManager;
            _caretaker = new Caretaker();
            _converter = new LilyPondConverter();

            musicManager.StaffChanged += this.OnSongLoaded;

            _mainViewModel = mainViewModel;
            //_musicLoader.LilypondViewModel = this;
            _text = "Your lilypond text will appear here.";

            ShortcutListener = new ShortcutListener();
            SetupShortcuts();
        }

        private void OnSongLoaded(object sender, EventArgs eventArgs)
        {

            _textChangedByLoad = true;
            var args = eventArgs as StaffChangedEventArgs;
            Song = args?.Staff;

            LilypondText = _converter.Convert(_song);
            _caretaker.Save(_text);
            _textChangedByLoad = false;
        }

        private void SetupShortcuts()
        {
            ShortcutListener.AddShortcut(new Key[] { Key.LeftAlt, Key.C }, new RelayCommand(() =>
            {
                TextBox.InsertAtCaretIndex("\\clef treble");
            }));
            ShortcutListener.AddShortcut(new Key[] { Key.LeftAlt, Key.S }, new RelayCommand(() =>
            {
                TextBox.InsertAtCaretIndex("\\tempo 4=120");
            }));
            ShortcutListener.AddShortcut(new Key[] { Key.LeftAlt, Key.D4 }, new RelayCommand(() =>
            {
                TextBox.InsertAtCaretIndex("\\time 4/4");
            }));
            ShortcutListener.AddShortcut(new Key[] { Key.LeftAlt, Key.D3 }, new RelayCommand(() =>
            {
                TextBox.InsertAtCaretIndex("\\time 3/4");
            }));
            ShortcutListener.AddShortcut(new Key[] { Key.LeftAlt, Key.D6 }, new RelayCommand(() =>
            {
                TextBox.InsertAtCaretIndex("\\time 6/8");
            }));
            ShortcutListener.AddShortcut(new Key[] { Key.LeftAlt, Key.T }, new RelayCommand(() =>
            {
                TextBox.InsertAtCaretIndex("\\time 4/4");
            }));
            ShortcutListener.AddShortcut(new Key[] { Key.LeftCtrl, Key.Z }, UndoCommand);
            ShortcutListener.AddShortcut(new Key[] { Key.LeftCtrl, Key.Y }, RedoCommand);
            ShortcutListener.AddShortcut(new Key[] { Key.LeftCtrl, Key.S }, SaveAsCommand);
            ShortcutListener.AddShortcut(new Key[] { Key.LeftCtrl, Key.L }, SaveAsLilypondCommand);
        }

        /*public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
            LilypondText = _previousText = text;
            _caretaker.Save(LilypondText);
            _textChangedByLoad = false;
        }
        */

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            // If we were typing, we need to do things.
            if (!_textChangedByLoad)
            {
                _waitingForRender = true;
                _lastChange = DateTime.Now;
               
                _mainViewModel.CurrentState = "Rendering...";

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        var failed = false;
                        
                        _waitingForRender = false;

                        _mainViewModel.CurrentState = "";
                        UndoCommand.RaiseCanExecuteChanged();
                        try
                        {
                            Song = _converter.ConvertBack(_text);
                        }
                        catch (Exception e)
                        {
                            failed = true;
                            _mainViewModel.CurrentState = "error";
                        }
                        if (!failed)
                        {
                            _musicManager.Staff = Song;
                            _caretaker.Save(_text);
                        }
                        
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _textChangedByLoad = true;
            LilypondText = _caretaker.Undo(_text);
            _textChangedByLoad = false;
            RedoCommand.RaiseCanExecuteChanged();
        }, () => _caretaker.CanUndo());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _textChangedByLoad = true;
            LilypondText = _caretaker.Redo(_text);
            _textChangedByLoad = false;
            UndoCommand.RaiseCanExecuteChanged();
        }, () => _caretaker.CanRedo());

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            SaveAs(_musicManager.GetSupportedFilesString());
        });

        public ICommand SaveAsLilypondCommand => new RelayCommand(() =>
        {
            SaveAs(LilypondFileHandler.GetSupportedFileTypeString());
        });

        private void SaveAs(string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = filter };
            if (saveFileDialog.ShowDialog() == true)
            {
                // TODO: check for success?
                _musicManager.SaveToFile(saveFileDialog.FileName);
            }
        }

        #endregion Commands for buttons like Undo, Redo and SaveAs

    }
}
