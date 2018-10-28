using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Messages
{
    public class EditorStatusMessage
    {

        public string Status { get; private set; }

        public EditorStatusMessage(string status)
        {
            Status = status;
        }

    }
}
