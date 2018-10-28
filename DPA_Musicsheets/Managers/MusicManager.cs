using DPA_Musicsheets.Events;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DPA_Musicsheets.Managers
{
    public class MusicManager
    {
        private Staff _staff;
        private readonly LinkedList<MusicFileHandler> _musicFileHandlers;
        private MusicFileHandler _fileHandler;

        public event EventHandler StaffChanged;

        public Staff Staff
        {
            get => _staff;
            set
            {
                _staff = value;
                OnStaffChanged(new StaffChangedEventArgs() { Staff = value });
            }
        }
        
        public MusicManager(LinkedList<MusicFileHandler> musicFileHandlers)
        {
            _musicFileHandlers = musicFileHandlers;
            _fileHandler = _musicFileHandlers.First();
        }

        protected virtual void OnStaffChanged(EventArgs e)
        {
            StaffChanged?.Invoke(this, e);
        }

        public void LoadFromFile(string filename)
        {
            Staff = _fileHandler.LoadFile(filename);
        }

        public bool SaveToFile(string filename)
        {
            return _fileHandler.SaveFile(filename, Staff);
        }

        public List<string> GetSupportedExtensions()
        {
            return _fileHandler.GetAllExtensions();
        }
    }
}
