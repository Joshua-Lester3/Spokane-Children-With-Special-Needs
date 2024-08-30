namespace SpokaneChildren.Api.Dtos;

public class ChangePasswordDto
{
	public required string Id { get; set; }
	public required string CurrentPassword { get; set; }
	public required string NewPassword { get; set; }
}