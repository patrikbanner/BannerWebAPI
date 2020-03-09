using System.Collections.Generic;
using BannerWebAPI.Models;

namespace BannerWebAPI.Repositories
{
    public interface IBannerRepository
    {
        public bool TrySave(in Banner banner);
        public bool TryGet(in int id, out Banner banner);
        bool Remove(in int bannerId);
        bool TryGetAll(out IEnumerable<Banner> banners);
    }
}
