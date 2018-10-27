using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Domain
{
    public abstract class StaffElement
    {
        public abstract void Accept(IStaffElementVisitor visitor);
    }
}
