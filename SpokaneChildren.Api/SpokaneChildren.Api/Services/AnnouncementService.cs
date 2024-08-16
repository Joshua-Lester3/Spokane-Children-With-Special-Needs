using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;

namespace SpokaneChildren.Api.Services;

public class AnnouncementService
{
	private readonly AppDbContext _context;

	public AnnouncementService(AppDbContext context)
	{
		_context = context;
	}

	public Announcement AddAnnouncement(AnnouncementDto dto)
	{
		Announcement addedAnnouncement = new()
		{
			Title = dto.Title,
			Description = dto.Description,
			DatePosted = DateTime.UtcNow,
		};
		_context.Add(addedAnnouncement);
		_context.SaveChanges();
		return addedAnnouncement;
	}
}
