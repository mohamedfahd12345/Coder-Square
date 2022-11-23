using System;
using System.Collections.Generic;

namespace coder_square.Models
{
    public partial class Follower
    {
        public int Id { get; set; }
        public string? FatherId { get; set; }
        public string? ChildId { get; set; }

        public virtual AspNetUser? Child { get; set; }
        public virtual AspNetUser? Father { get; set; }
    }
}
