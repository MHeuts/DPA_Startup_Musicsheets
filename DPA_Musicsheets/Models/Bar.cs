﻿using System.Collections.Generic;

namespace DPA_Musicsheets.Models
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
