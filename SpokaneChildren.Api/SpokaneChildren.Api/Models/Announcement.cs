using System.ComponentModel.DataAnnotations;

namespace SpokaneChildren.Api.Models;

public class Announcement
{
	public int Id { get; set; }
	[Required]
	public required string Title { get; set; }
	public string? Description { get; set; }
	[Required]
	public required DateTime DatePosted { get; set; }
}
