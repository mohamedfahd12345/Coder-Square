using System;
using System.Collections.Generic;

namespace coder_square.Models
{
    public partial class SavedDetail
    {
        public int SavedDetailsId { get; set; }
        public int? PostId { get; set; }
        public int? SavedId { get; set; }

        public virtual Post? Post { get; set; }
        public virtual Saved? Saved { get; set; }
    }
}
