using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using AmDmSite.Models.SiteDataBase;

namespace AmDmSite.Cache
{
    public class SiteCache
    {
        public int Count()
        {
            return MemoryCache.Default.Count();
        }

        public Performer GetValue(int id)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(id.ToString()) as Performer;
        }

        public bool Add(Performer value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return (memoryCache.Add(value.Id.ToString(), value, DateTime.Now.AddMinutes(10)));
        }

        public void Update(Performer value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(value.Id.ToString(), value, DateTime.Now.AddMinutes(10));
        }

        public void Delete(int id)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Contains(id.ToString());
        }
    }
}