namespace SpokaneChildren.Api.Dtos;

public class UserInfoDto
{
	public required string UserId { get; set; }
	public string? UserName { get; set; }
	public string? Email { get; set; }
	public string[] Roles { get; set; } = [];
}
