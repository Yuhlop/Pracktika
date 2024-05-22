using Microsoft.AspNetCore.Mvc;
using Prackticheskaya_2024.News;
using Prackticheskaya_2024.Users;
using System.ComponentModel.DataAnnotations;

namespace Prackticheskaya_2024.Comments
{
    public class CommentsCreateDTO
    {
        [Required]
        [MinLength(3)]
        public string Text { get; set; }
        [Required]
        public int NewsId {  get; set; }
    }
    public class CommentsDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public NewsDTO News { get; set; }
        public UserDTO User { get; set; }
    }
}
