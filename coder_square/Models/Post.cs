using System;
using System.Collections.Generic;

namespace coder_square.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            LikesNavigation = new HashSet<Like>();
            SavedDetails = new HashSet<SavedDetail>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public DateTime? Date { get; set; }
        public int? Likes { get; set; }
        public string? UserId { get; set; }
        public int? NumComments { get; set; }

        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> LikesNavigation { get; set; }
        public virtual ICollection<SavedDetail> SavedDetails { get; set; }
    }
}
