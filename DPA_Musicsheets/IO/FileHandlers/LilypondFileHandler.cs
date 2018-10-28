using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.IO
{
    class LilypondFileHandler : MusicFileHandler
    {
        public List<string> Extensions { get; private set; }

        public LilypondFileHandler()
        {
            Extensions = new List<string> { "ly" };
        }

        protected override Staff Load(string filename)
        {
            var converter = new LilyPondConverter();
            StringBuilder sb = new StringBuilder();

            foreach (var line in File.ReadAllLines(filename))
            {
                sb.AppendLine(line);
            }

            return converter.ConvertBack(sb.ToString());
        }

        protected override bool Save(string filename, Staff staff)
        {
            throw new NotImplementedException();
        }
    }
}
