using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{


    public class UserStore : StoreBase<User>, IUserStore<User> 
    {

        public UserStore(QuzeContext ctx):base(ctx)
        {
           
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ctx?.Dispose();
            }
        }

        #region createuser
        public async Task<IdentityResult> CreateAsync(User user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            try
            {
                await ctx.Users.AddAsync(user);
                var recordsAffected = await ctx.SaveChangesAsync(cancellationToken);
                if (recordsAffected > 0)
                    return IdentityResult.Success;
                return IdentityResult.Failed(new IdentityError() { Code = "1", Description = "no record added" });
            }
            catch (Exception ex)
            {

                //return IdentityResult.Failed(new IdentityError() { Code = "2", Description = ex.Message+" "+ex.InnerException.Message });
                throw ex;
            }
        }
        #endregion

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }


        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(SetUserNameAsync));
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(GetNormalizedUserNameAsync));
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

     

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            ctx.Entry(user).State = EntityState.Modified;

            var i = await ctx.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            user.IsDeleted = true;
            ctx.Entry(user).State=EntityState.Modified;

            var i = await ctx.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<User> FindByIdAsync(string userId,
                CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new ArgumentException("use FindByIdAsync(string userId, int userType, CancellationToken cancellationToke) with userType parameter");
        }
        public async Task<User> FindByIdAsync(string userId, int userType,
                CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            int intId;
            if (!int.TryParse(userId, out intId)) throw new ArgumentException("userId is not numeric");

            return await ctx.Users.Include(u=>u.Fellows)
                .Where(u=>u.Id==intId
                && u.UserTypeId == userType)
                .FirstOrDefaultAsync();

        }

        public async Task<User> FindByNameAsync(string userName,
                CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new ArgumentException("use FindByNameAsync(string userName, int userType, CancellationToken cancellationToke) with userType parameter");

        }
        public async Task<User> FindByNameAsync(string userName, int userType,
                CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            return await ctx.Users.FirstOrDefaultAsync(
                u => u.UserName == userName
                && u.UserTypeId == userType);
        }

    }
}

