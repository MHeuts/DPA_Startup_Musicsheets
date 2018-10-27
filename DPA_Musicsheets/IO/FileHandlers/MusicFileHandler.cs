﻿using DPA_Musicsheets.Models;
using System.Collections.Generic;
using System.IO;

namespace DPA_Musicsheets.IO.FileHandlers
{
    public abstract class MusicFileHandler
    {

        public MusicFileHandler()
        {
            Extensions = new List<string>();
        }

        public List<string> Extensions { get; protected set; }
        public MusicFileHandler Next { get; set; }

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

        public List<string> GetAllExtensions()
        {
            var ext = Extensions;
            if (Next != null)
            {
                ext.AddRange(Next.Extensions);
            }
            return ext;
        }

    }
}
