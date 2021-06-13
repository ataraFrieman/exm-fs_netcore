using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{

    /// <summary>
    /// This cache keeps reco
    /// </summary>
    public class CacheBase<T> where T : EntityBase
    {
        // protected User CurrentUser { get; set; }
        private QuzeContext ctx { get; set; }
        private static readonly List<PropertyInfo> QuzeCtxProps;

        private static readonly MemoryCacheOptions options = new MemoryCacheOptions();
        private static readonly MemoryCache cache = new MemoryCache(options);

        static CacheBase()
        {
            QuzeCtxProps = typeof(QuzeContext).GetProperties().ToList();
        }

        public CacheBase(QuzeContext ctx)
        {
            this.ctx = ctx;

            var list = new List<T>();
            var entityType = typeof(T);

            if (cache.TryGetValue(entityType.Name, out list))
                return;


            // if the table isn't in the cache
            //try
            //{
            //    var Data = DbSetT.ToList();
            //    if (Data != null)
            //        cache.Set(entityType.Name, Data);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


        protected void AddOrUpdateCache(T entity)
        {
            var index = Data.IndexOf(entity);
            if (index > -1)
            {
                Data[index] = entity;
            }
            else
            {
                Data.Add(entity);
            }
        }

        public async Task<int> CreateAsync(T entity)
        {
            int recordsAffected;
            try
            {
                DbSetT.Add(entity);
                recordsAffected = await ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Data.Add(entity);

            return recordsAffected;
        }

        public async Task<int> CreateAsync(List<T> entities)
        {
            int recordsAffected;
            try
            {
                await DbSetT.AddRangeAsync(entities);
                recordsAffected = await ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Data.AddRange(entities);

            return recordsAffected;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            //update db
            ctx.Entry(entity).State = EntityState.Modified;

            var recordsAffected = await ctx.SaveChangesAsync();

            if (recordsAffected > 0)
            {
                // update cache
                AddOrUpdateCache(entity);
            }
            return recordsAffected;
        }

        //TODO: this must be done in a transaction....
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<T> entities)
        {
            var recordsAffected = 0;
            try
            {
                foreach (var item in entities)
                {
                    recordsAffected += await UpdateAsync(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Data.AddRange(entities);

            return recordsAffected;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = Data.FirstOrDefault(x => x.Id == id);
            return await DeleteAsync(entity);
        }



        public async Task<int> DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            int recordsAffected;
            var originalIsDeleted = entity.IsDeleted;
            entity.IsDeleted = true;
            try
            {
                ctx.Entry(entity).State = EntityState.Modified;
                recordsAffected = await ctx.SaveChangesAsync();
            }
            catch (Exception)
            {
                entity.IsDeleted = originalIsDeleted;
                throw;
            }

            if (Data.Contains(entity))
                Data.Remove(entity);

            return recordsAffected;
        }


        /// <summary>
        /// Returns all entities where isDeleted==false
        /// </summary>
        public List<T> Data
        {
            get
            {
                var entityType = typeof(T);

                // if is in cache get from cache
                if (cache.TryGetValue(entityType.Name, out List<T> list))
                    return list;

                // else get from db
                list = DbSetT.Where(t => t.IsDeleted == false).ToList();
                cache.Set(entityType.Name, list);

                return list;
            }
        }

        // gets the value of Dbset<T>
        private DbSet<T> DbSetT
        {
            get
            {
                var dbSetT = QuzeCtxProps.FirstOrDefault(p => p.PropertyType == typeof(DbSet<T>));
                var temp = dbSetT.GetValue(ctx) as DbSet<T>;
                return temp;
            }
        }



    }
}
