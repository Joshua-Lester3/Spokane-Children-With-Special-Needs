using System.ComponentModel.DataAnnotations;

namespace SpokaneChildren.Api.Dtos;

public class EventDto
{
	public int EventId { get; set; } = -1;
	public string EventName { get; set; }
	public string Description { get; set; }
	public DateTime DateTime { get; set; }
	public string Location { get; set; }
	public string? Link { get; set; }
}
