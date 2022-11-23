using System;
using System.Collections.Generic;

namespace coder_square.Models
{
    public partial class Saved
    {
        public Saved()
        {
            SavedDetails = new HashSet<SavedDetail>();
        }

        public int SavedId { get; set; }
        public string? UserId { get; set; }
        public string? SavedName { get; set; }

        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<SavedDetail> SavedDetails { get; set; }
    }
}
