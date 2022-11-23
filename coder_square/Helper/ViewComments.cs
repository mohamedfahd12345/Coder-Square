public class Comments
{
    public string? description { get; set; }
    public string? user_name { get; set; }
    public DateTime? Date { get; set; }
}
namespace coder_square.Helper
{
    public class ViewComments
    {
        public int? post_id { get; set; }

        public  List<Comments> allcomments { get; set; }
        
    }
}
