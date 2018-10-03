using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.IO
{
    public interface IMusicFileHandler
    {

        Song LoadFile(string fileName);
        void SaveFile(string fileName, Song song);

        List<string> Extensions { get; }

    }
}
