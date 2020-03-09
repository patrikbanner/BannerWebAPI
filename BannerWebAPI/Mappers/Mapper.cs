using System;
using BannerWebAPI.Models;

namespace BannerWebAPI.Mappers
{
    public class Mapper : IMapper
    {
        public Banner Map(in PostBanner postBanner)
        {
            return new Banner
            {
                Id = postBanner.Id,
                Html = postBanner.Html,
                Created = DateTime.UtcNow
            };
        }

        public Banner Map(in Banner banner, in PutBanner putBanner)
        {
            banner.Html = putBanner.Html;
            banner.Modified = DateTime.UtcNow;

            return banner;
        }
    }
}
