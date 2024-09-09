namespace SpokaneChildren.Api.Identity;

public class Roles
{
	public const string Admin = "Admin";
	public const string Moderator = "Moderator";
	public const string None = "None";
	public static readonly string[] RolesList = { Admin, Moderator, None };
}
