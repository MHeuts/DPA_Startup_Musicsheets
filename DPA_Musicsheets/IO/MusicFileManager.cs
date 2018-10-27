using DPA_Musicsheets.IO.Midi;
using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.IO
{
    public class MusicFileManager
    {
        public event EventHandler SongLoaded;

        private List<IMusicFileHandler> _handlers;

        public MusicFileManager()
        {
            _handlers = new List<IMusicFileHandler> {
                new MidiFileHandler(),
                new LilypondFileHandler()
            };
        }

        protected virtual void OnSongLoaded(EventArgs e)
        {
            SongLoaded?.Invoke(this, e);
        }

        public Staff Load(string filename)
        {
            Staff song = null;
            string extension = filename.Split('.').Last();
            foreach (var handler in _handlers)
            {
                if (handler.Extensions.Contains(extension))
                {
                    song = handler.LoadFile(filename);
                    break;
                }
            }
            OnSongLoaded(new SongLoadedEventsArgs() { Song = song });
            return song;
        }
    }
}
