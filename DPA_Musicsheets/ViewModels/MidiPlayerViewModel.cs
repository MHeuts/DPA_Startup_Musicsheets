﻿using DPA_Musicsheets.IO;
using DPA_Musicsheets.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;

namespace DPA_Musicsheets.ViewModels
{
    /// <summary>
    /// The viewmodel for playing midi sequences.
    /// It supports starting, stopping and restarting.
    /// </summary>
    public class MidiPlayerViewModel : ViewModelBase
    {
        private MusicPlayer _musicPlayer;
        private Staff _staff;

        public Staff Staff
        {
            get { return _staff; }
            set
            {
                _musicPlayer.Staff = value;
                _staff = value;
            }
        }

        public MidiPlayerViewModel(MusicPlayer musicPlayer, MusicFileManager fileManager)
        {
            _musicPlayer = musicPlayer;
            fileManager.SongLoaded += OnSongLoaded;
            musicPlayer.StatusChanged += OnStatusChanged;
        }

        private void OnSongLoaded(object sender, EventArgs eventArgs)
        {
            var args = eventArgs as SongLoadedEventsArgs;
            Staff = args?.Song;
        }

        private void OnStatusChanged(object sender, EventArgs eventArgs)
        {
            var args = eventArgs as MusicPlayerStatusChangedEventArgs;
            PlayCommand.RaiseCanExecuteChanged();
            PauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        #region buttons for play, stop, pause
        public RelayCommand PlayCommand => new RelayCommand(() =>
        {
            _musicPlayer.Play();
        }, () => !_musicPlayer.Running && _staff != null);

        public RelayCommand StopCommand => new RelayCommand(() =>
        {
            _musicPlayer.Stop();
        }, () => _musicPlayer.Running);

        public RelayCommand PauseCommand => new RelayCommand(() =>
        {
            _musicPlayer.Pause();
        }, () => _musicPlayer.Running);

        #endregion buttons for play, stop, pause

    }
}
