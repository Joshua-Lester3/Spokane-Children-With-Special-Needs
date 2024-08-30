namespace SpokaneChildren.Api.Dtos;

public class UpdateUserInfoDto
{
	public required string Id { get; set; }
	public required string NewUsername { get; set; }
	public required string NewRole { get; set; }
}
