using DPA_Musicsheets.IO;
using DPA_Musicsheets.Models;
using GalaSoft.MvvmLight;
using System;

namespace DPA_Musicsheets.ViewModels
{
    public class StaffsViewModel : ViewModelBase
    {
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
