namespace Quze.Infrastruture.Security
{
    public interface IUserService
    {
        int GetCurrentUserId();
        string GetCurrentUserName();
        IUser GetCurrentUser();
    }
}
