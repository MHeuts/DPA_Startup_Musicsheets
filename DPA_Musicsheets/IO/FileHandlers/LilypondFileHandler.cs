using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.IO.FileHandlers
{
    class LilypondFileHandler : MusicFileHandler
    {
        private LilyPondConverter _converter;

        public LilypondFileHandler(LilyPondConverter converter, MusicFileHandler next = null) : base(next)
        {
            _converter = converter;
        }

        public static List<string> GetExtensions()
        {
            return new List<string> { ".ly" };
        }

        public static string GetFileType()
        {
            return "LilyPond";
        }

        public static string GetSupportedFileTypeString()
        {
            return MusicFileHandler.BuildSupportedFileTypeString(GetFileType(), GetExtensions());
        }

        public override List<string> Extensions => LilypondFileHandler.GetExtensions();

        public override string FileType => GetFileType();

        public override string FileTypeString => GetSupportedFileTypeString();

        protected override Staff Load(string filename)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var line in File.ReadAllLines(filename))
            {
                sb.AppendLine(line);
            }

            return _converter.ConvertBack(sb.ToString());
        }

        protected override bool Save(string filename, Staff staff)
        {
            var lily = _converter.Convert(staff);
            if (lily == null) return false;

            try
            {
                System.IO.File.WriteAllText(@filename, lily);
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
