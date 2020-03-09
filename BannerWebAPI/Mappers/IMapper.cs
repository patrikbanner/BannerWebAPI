using BannerWebAPI.Models;

namespace BannerWebAPI.Mappers
{
    public interface IMapper
    {
        public Banner Map(in PostBanner postBanner);
        public Banner Map(in Banner banner, in PutBanner putBanner);
    }
}
