using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class UserTasksStore : StoreBase<UserTask>
    {
        private readonly QuzeContext db;

        public UserTasksStore(QuzeContext ctx):base(ctx)
        {
            db = ctx;
        }

     
        public async override Task<int> CreateAsync(UserTask userTask)
        {
            if (userTask.TaskType == 0) userTask.TaskType = TaskType.Task;
            DbSetT.Add(userTask);
            return await ctx.SaveChangesAsync();
        }

        /// <summary>
        /// Fetch all userTasks of the given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserTask>> GetAll(int userId)
        {
            if (userId == 0) throw new ArgumentException("Id must be provided");
            var list =  await db.UserTasks.Where(ut=>ut.UserId==userId).ToListAsync();
            return list;
        }

        public async Task<UserTask> UpdateUserTask(UserTask userTask)
        {
            var entry= db.Entry<UserTask>(userTask);
            entry.State = EntityState.Modified;
            await db.SaveChangesAsync();
            return userTask;
        }
    }
}
