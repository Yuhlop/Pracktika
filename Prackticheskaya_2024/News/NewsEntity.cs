using Prackticheskaya_2024.Users;

namespace Prackticheskaya_2024.News
{
    public class NewsEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
    }
}
