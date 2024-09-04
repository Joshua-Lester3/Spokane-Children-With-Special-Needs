using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SpokaneChildren.Api.Models;

[Index(nameof(DatePosted))]
public class Announcement
{
	public int Id { get; set; }
	[Required]
	public required string Title { get; set; }
	public string? Description { get; set; }
	[Required]
	public required DateTime DatePosted { get; set; }
}
