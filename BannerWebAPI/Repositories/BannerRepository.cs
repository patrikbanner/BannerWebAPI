using System.Collections.Generic;
using System.Linq;
using BannerWebAPI.Models;

namespace BannerWebAPI.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly Dictionary<int, Banner> _storage = new Dictionary<int, Banner>();

        public bool TryGet(in int id, out Banner banner)
        {
            return _storage.TryGetValue(id, out banner);
        }

        public bool TryGetAll(out IEnumerable<Banner> banners)
        {
            banners = _storage.Values;

            return banners.Any();
        }

        public bool TrySave(in Banner banner)
        {
            if (_storage.ContainsKey(banner.Id))
            {
                _storage[banner.Id] = banner;
                return true;
            }

            return _storage.TryAdd(banner.Id, banner);
        }

        public bool Remove(in int bannerId)
        {
            return _storage.Remove(bannerId);
        }
    }
}
