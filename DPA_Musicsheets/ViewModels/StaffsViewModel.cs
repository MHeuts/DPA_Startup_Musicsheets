using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DPA_Musicsheets.ViewModels
{
    public class StaffsViewModel : ViewModelBase
    {
        private Song _song;
        public Song Song
        {
            get { return _song; }
            set
            {
                _song = value;
                RaisePropertyChanged("Song");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="musicLoader">We need the musicloader so it can set our staffs.</param>
        public StaffsViewModel(MusicFileManager fileManager)
        {
            fileManager.SongLoaded += this.OnSongLoaded;
        }

        private void OnSongLoaded(object sender, EventArgs eventsArgs)
        {
            var args = eventsArgs as SongLoadedEventsArgs;
            Song = args?.Song;
        }
        
    }
}
