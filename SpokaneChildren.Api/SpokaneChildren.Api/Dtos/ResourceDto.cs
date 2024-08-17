using SpokaneChildren.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace SpokaneChildren.Api.Dtos;

public class ResourceDto
{
	public int ResourceId { get; set; } = -1;
	public ResourceCategory? Category { get; set; }
	public string Name { get; set; }
	public string? Website { get; set; }
	public string? Phone { get; set; }
	public string? Address { get; set; }
}
