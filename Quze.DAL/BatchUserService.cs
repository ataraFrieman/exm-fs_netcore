using Quze.Infrastruture.Security;

namespace Quze.DAL
{

    public class BatchUserService : IUserService
    {
        int userId;
        public BatchUserService(int userId)
        {
            this.userId = userId;
        }

        public int GetCurrentUserId()
        {
            return userId;
        }

        public string GetCurrentUserName()
        {
            return "System user";
        }


        IUser IUserService.GetCurrentUser()
        {
            throw new System.NotImplementedException();
        }
    }
}
