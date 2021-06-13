using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{

    /// <summary>
    /// General DAL methods
    /// </summary>
    public class StoreBase<T> where T : EntityBase
    {
        protected User CurrentUser { get; set; }
        public QuzeContext ctx { get; set; }
        protected string ConnectionString { get; set; }
        // protected EntityBase entityBase { get; set; }
        protected static readonly List<PropertyInfo> properties;
        protected readonly CacheBase<T> Cache;

        

       
        static StoreBase()
        {
            properties = typeof(QuzeContext).GetProperties().ToList();
        }

        

        protected StoreBase(QuzeContext ctx)
        {
            this.ctx = ctx;
            Cache  = new CacheBase<T>(ctx);
        }

        protected DbSet<T> DbSetT
        {
            get
            {
                var property = properties.FirstOrDefault(p => p.PropertyType == typeof(DbSet<T>));
                var temp = property.GetValue(ctx) as DbSet<T>;
                return temp;
            }
        }

        public virtual async Task<int> CreateAsync(List<T> entities)
        {
            return await Cache.CreateAsync(entities);

        }

        public virtual async Task<int> CreateAsync(T entity)
        {
           return await Cache.CreateAsync(entity);
          
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await Cache.DeleteAsync(id);
        }

        public async Task<int> DeleteAsync(T entity)
        {
          return await Cache.DeleteAsync(entity);
        }

        public async Task<int> SaveAsync(T entity)
        {
             return await Cache.UpdateAsync(entity);
        }

        public async Task<int> SaveAsync(List<T> entities)
        {
            return await Cache.UpdateAsync(entities);
        }


        public async  Task<T> GetByIdAsync(int id)
        {
           return Cache.Data.FirstOrDefault(e => e.Id == id);
        }


        public virtual async Task<List<T>> ToListAsync()
        {
            return Cache.Data.ToList();
        }
        

    }
}
