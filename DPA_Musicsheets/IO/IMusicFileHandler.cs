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

        Staff LoadFile(string fileName);
        void SaveFile(string fileName, Staff song);

        List<string> Extensions { get; }

    }
}
