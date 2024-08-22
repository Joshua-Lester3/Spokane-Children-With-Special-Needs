using Microsoft.EntityFrameworkCore;
using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;

namespace SpokaneChildren.Api.Services;

public class AnnouncementService
{
	private readonly AppDbContext _context;
	private static object _deletingAnnouncementLock = new();
	private static object _postingAnnouncementLock = new();

	public AnnouncementService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<Announcement> AddAnnouncement(AnnouncementDto dto)
	{
		if (dto.Title is null)
		{
			throw new ArgumentNullException(nameof(dto.Title));
		}
		if (dto.Title.Trim().Length == 0)
		{
			throw new ArgumentException($"{nameof(dto.Title)} is not allowed to be empty.");
		}

		Announcement? foundAnnouncement = null;
		if (dto.Id != -1)
		{
			foundAnnouncement = await _context.Announcements
				.FirstOrDefaultAsync(announcement => announcement.Id == dto.Id);
		}
		if (foundAnnouncement is null)
		{
			lock (_postingAnnouncementLock)
			{
				if (dto.Id != -1)
				{
					foundAnnouncement = _context.Announcements
						.FirstOrDefault(announcement => announcement.Id == dto.Id);
				}

				if (foundAnnouncement is null)
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
				else
				{
					foundAnnouncement.Title = dto.Title;
					foundAnnouncement.Description = dto.Description;
					foundAnnouncement.DatePosted = DateTime.UtcNow;
					_context.SaveChanges();
					return foundAnnouncement;
				}
			}
		}
		else
		{
			lock (_postingAnnouncementLock)
			{
				foundAnnouncement.Title = dto.Title;
				foundAnnouncement.Description = dto.Description;
				foundAnnouncement.DatePosted = DateTime.UtcNow;
				_context.SaveChanges();
				return foundAnnouncement;
			}
		}
	}

	public async Task<bool> DeleteAnnouncement(int id)
	{
		var foundAnnouncment = await _context.Announcements.FirstOrDefaultAsync(announcement => announcement.Id == id);
		if (foundAnnouncment is not null)
		{
			lock (_deletingAnnouncementLock)
			{
				foundAnnouncment = _context.Announcements.FirstOrDefault(announcement => announcement.Id == id);
				if (foundAnnouncment is not null)
				{
					_context.Announcements.Remove(foundAnnouncment);
					_context.SaveChanges();
					return true;
				}
			}
		}
		return false;
	}

	public async Task<List<Announcement>> GetAnnouncementList(int page, int countPerPage)
	{
		if (page < 0)
		{
			throw new ArgumentException($"{nameof(page)} cannot be less than 0.");
		}
		if (countPerPage < 1)
		{
			throw new ArgumentException($"{nameof(countPerPage)} cannot be less than 1.");
		}
		int skip = page * countPerPage;
		return await _context.Announcements
			.OrderByDescending(announcement => announcement.DatePosted)
			.Skip(skip)
			.Take(countPerPage)
			.ToListAsync();
	}

	public async Task<Announcement?> GetAnnouncement(int id)
	{
		return await _context.Announcements.FirstOrDefaultAsync(a => a.Id == id);
	}
}
