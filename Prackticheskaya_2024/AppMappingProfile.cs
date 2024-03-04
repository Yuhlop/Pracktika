using Prackticheskaya_2024.News;
using Prackticheskaya_2024.Users;

namespace Prackticheskaya_2024
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserEntity, UserDTO>();
            CreateMap<NewsEntity, NewsDTO>();
        }
    }
}
