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

        public List<Performer> GetPerformers()
        {
            return MemoryCache.Default.Get("performers") as List<Performer>;
        }
       
        public void UpdatePerformers(List<Performer> performers)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set("performers", performers, DateTime.Now.AddMinutes(10));
        }


        public Performer GetValue(int id)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(id.ToString()) as Performer;
        }

        public bool Add(Performer value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Add(value.Id.ToString(), value, DateTime.Now.AddMinutes(10))) {
                List<Performer> performers = GetPerformers();
                performers.Add(value);
                UpdatePerformers(performers);
                return true;
            }
            return false;
        }

        public void Update(Performer value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(value.Id.ToString(), value, DateTime.Now.AddMinutes(10));
            Performer performer = GetPerformers().FirstOrDefault(x => x.Id == value.Id);
            List<Performer> performers = GetPerformers();
            performers.Remove(performer);
            performers.Add(value);
            UpdatePerformers(performers);
        }

        public void Delete(int id)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Contains(id.ToString());
            { 
             memoryCache.Remove(id.ToString()); 
             List<Performer> performers = GetPerformers(); 
             performers.Remove(performers.FirstOrDefault(x => x.Id == id)); 
             UpdatePerformers(performers); 
            } 
        }

        public void UpdateLastPerformerId(int value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set("perf", value, DateTime.Now.AddMinutes(90));
        }

        public int GetLastPerformerId()
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return (int)memoryCache.Get("perf");
        }
        
    }
}