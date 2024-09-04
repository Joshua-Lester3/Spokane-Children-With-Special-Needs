using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpokaneChildren.Api.Models;

[Index(nameof(DateTime))]
public class Event
{
	public int EventId { get; set; }
	[Required]
	public required string EventName { get; set; }
	[Required]
	public required string Description { get; set; }
	public DateTime DateTime { get; set; }
	[Required]
	public required string Location { get; set; }
	public string? Link { get; set; }
}
