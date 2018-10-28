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

        public LilypondFileHandler(LilyPondConverter converter, MusicFileHandler next = null) : base("LilyPond", next)
        {
            _converter = converter;
            Extensions.Add(".ly");
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
