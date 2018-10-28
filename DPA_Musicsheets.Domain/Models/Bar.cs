using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Domain
{
    public class Bar : StaffElement
    {
        public Bar()
        {
            MusicNotes = new List<MusicNote>();
        }

        public List<MusicNote> MusicNotes { get; set; }

        public override void Accept(IStaffElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
