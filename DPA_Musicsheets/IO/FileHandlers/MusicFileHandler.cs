using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DPA_Musicsheets.IO.FileHandlers
{
    public abstract class MusicFileHandler
    {

        public MusicFileHandler(string fileType, MusicFileHandler next = null)
        {
            Extensions = new List<string>();
            Next = next;
            FileType = fileType;
        }

        public List<string> Extensions { get; protected set; }
        public string FileType { get; protected set; }
        public MusicFileHandler Next { get; protected set; }

        public Staff LoadFile(string fileName)
        {
            if (HandlesFile(fileName))
            {
                return Load(fileName);
            }
            if (Next != null)
            {
                return Next.LoadFile(fileName);
            }
            return null;
        }

        public bool SaveFile(string fileName, Staff staff)
        {
            if (HandlesFile(fileName))
            {
                return Save(fileName, staff);
            }
            if (Next != null)
            {
                return Next.SaveFile(fileName, staff);
            }
            return false;
        }

        protected abstract Staff Load(string filename);
        protected abstract bool Save(string filename, Staff staff);

        public bool HandlesFile(string filename)
        {
            return Extensions.Contains(Path.GetExtension(filename));
        }

        public List<Tuple<string, List<string>>> GetSupportedFiles()
        {
            var ext = new List<Tuple<string, List<string>>>
            {
                new Tuple<string, List<string>>(FileType, Extensions)
            };

            if (Next != null)
            {
                ext.AddRange(Next.GetSupportedFiles());
            }
            return ext;
        }

    }
}
