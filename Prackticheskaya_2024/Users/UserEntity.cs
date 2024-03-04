namespace Prackticheskaya_2024.Users
{
    public class UserEntity
    {
            public int Id { get; set; }
            public string Family { get; set; } = "";
            public string Name { get; set; } = "";
            public string Patronymic { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
    }
}
