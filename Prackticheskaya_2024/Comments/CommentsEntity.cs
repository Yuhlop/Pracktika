using Prackticheskaya_2024.News;
using Prackticheskaya_2024.Users;
namespace Prackticheskaya_2024.Comments
{
    public class CommentsEntity
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public NewsEntity News { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
