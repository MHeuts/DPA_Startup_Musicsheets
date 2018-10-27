using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.IO
{
    class LilypondFileHandler : IMusicFileHandler
    {
        public List<string> Extensions { get; private set; }

        public LilypondFileHandler()
        {
            Extensions = new List<string> { "ly" };
        }
        public Staff LoadFile(string fileName)
        {



            var converter = new LilyPondConverter();
            StringBuilder sb = new StringBuilder();

            foreach(var line in File.ReadAllLines(fileName))
            {
                sb.AppendLine(line);
            }

            return converter.ConvertBack(sb.ToString());
        }

        public void SaveFile(string fileName, Staff song)
        {
            throw new NotImplementedException();
        }
    }
}
