using System;
using System.Collections.Generic;

namespace coder_square.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? PostId { get; set; }
        public string? Descripiton { get; set; }
        public DateTime? Date { get; set; }

        public virtual Post? Post { get; set; }
        public virtual AspNetUser? User { get; set; }
    }
}
