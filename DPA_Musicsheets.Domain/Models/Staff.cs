using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Domain
{
    public class Staff : StaffElement
    {
        public Staff()
        {
            Children = new List<StaffElement>();
        }
        
        public Staff Parent { get; set; }
        public List<StaffElement> Children { get; private set; }
        public Tuple<int, int> Rhythm { get; set; }
        public int Bpm { get; set; }

        public override void Accept(IStaffElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
