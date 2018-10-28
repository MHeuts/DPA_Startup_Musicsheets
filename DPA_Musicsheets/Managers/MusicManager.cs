using DPA_Musicsheets.Events;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPA_Musicsheets.Managers
{
    public class MusicManager
    {
        private Staff _staff;
        private readonly MusicFileHandler _fileHandler;

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
        
        public MusicManager(MusicFileHandler musicFileHandler)
        {
            _fileHandler = musicFileHandler;
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

        public string GetSupportedFilesString()
        {
            var builder = new StringBuilder();
            var supported = _fileHandler.GetSupportedFiles();
            foreach (var filetype in supported)
            {
                builder.Append($"{filetype.Item1} (");
                foreach (var extension in filetype.Item2)
                {
                    builder.Append($"*{extension} ");
                }
                builder.Append(")|");
                foreach (var extension in filetype.Item2)
                {
                    builder.Append($"*{extension}");
                    if (extension != filetype.Item2.Last()) builder.Append(";");
                }
                if (filetype != supported.Last()) builder.Append("|");
            }

            return builder.ToString();
        }
    }
}
