using System.ComponentModel.DataAnnotations;

namespace SpokaneChildren.Api.Models;

public class Resource
{
	public int ResourceId { get; set; }
	public ResourceCategory? Category { get; set; }
	[Required]
	public required string Name { get; set; }
	public string? Website { get; set; }
	public string? Phone { get; set; }
	public string? Address { get; set; }
}

public enum ResourceCategory
{
	Therapy,
	Psychiatrist,
	// Ask what else there is
}