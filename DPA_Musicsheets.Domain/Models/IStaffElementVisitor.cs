﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Domain
{
    public interface IStaffElementVisitor
    {
        void Visit(Staff staff);
        void Visit(Bar bar);

    }
}
