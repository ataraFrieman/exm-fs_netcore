namespace Quze.Infrastruture.Security
{
    /// <summary>
    /// Represents security logedin user (not the entity - "User")
    /// </summary>
    public interface IUser
    {
        int Id { get; set; }
        string UserName { get; set; }
        int? OrganizationId { get; set; }
        int UserTypeId { get; set; }
        string   IdentityNumber { get; set; }
    }
}
