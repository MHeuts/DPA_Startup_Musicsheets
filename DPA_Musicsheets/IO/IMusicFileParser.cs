using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.IO
{
    public interface IMusicFileParser
    {

        Song parseFile(string fileName);

    }
}
