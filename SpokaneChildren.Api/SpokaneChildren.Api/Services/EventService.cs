using Microsoft.EntityFrameworkCore;
using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;

namespace SpokaneChildren.Api.Services;

public class EventService
{
	private readonly AppDbContext _context;
	private static object _deletingEventLock = new();
	private static object _postingEventLock = new();

	public EventService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<Event> PostEvent(EventDto dto)
	{
		if (dto.EventName is null)
		{
			throw new ArgumentNullException(nameof(dto.EventName));
		}
		if (dto.EventName.Trim().Length == 0)
		{
			throw new ArgumentException($"{nameof(dto.EventName)} is not allowed to be empty.");
		}
		if (dto.Description is null)
		{
			throw new ArgumentNullException(nameof(dto.Description));
		}
		if (dto.Description.Trim().Length == 0)
		{
			throw new ArgumentException($"{nameof(dto.Description)} is not allowed to be empty.");
		}
		if (dto.Location is null)
		{
			throw new ArgumentNullException(nameof(dto.Location));
		}
		if (dto.Location.Trim().Length == 0)
		{
			throw new ArgumentException($"{nameof(dto.Location)} is not allowed to be empty.");
		}
		if (dto.DateTime.Equals(DateTime.MinValue))
		{
			throw new ArgumentException($"{nameof(dto.DateTime)} is not allowed to be default value (01/01/0001).");
		}

		Event? foundEvent = null;
		if (dto.EventId != -1)
		{
			foundEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == dto.EventId);
		}
		if (foundEvent is null)
		{
			lock (_postingEventLock)
			{
				if (dto.EventId != -1)
				{
					foundEvent = _context.Events
						.FirstOrDefault(e => e.EventId == dto.EventId);
				}

				if (foundEvent is null)
				{
					Event addedEvent = new()
					{
						EventName = dto.EventName,
						Description = dto.Description,
						DateTime = dto.DateTime,
						Location = dto.Location,
						Link = dto.Link,
					};
					_context.Add(addedEvent);
					_context.SaveChanges();
					return addedEvent;
				}
				else
				{
					foundEvent.EventName = dto.EventName;
					foundEvent.Description = dto.Description;
					foundEvent.DateTime = dto.DateTime;
					foundEvent.Location = dto.Location;
					foundEvent.Link = dto.Link;
					_context.SaveChanges();
					return foundEvent;
				}
			}
		}
		else
		{
			lock (_postingEventLock)
			{
				foundEvent.EventName = dto.EventName;
				foundEvent.Description = dto.Description;
				foundEvent.DateTime = dto.DateTime;
				foundEvent.Location = dto.Location;
				foundEvent.Link = dto.Link;
				_context.SaveChanges();
				return foundEvent;
			}
		}
	}

	public async Task<bool> DeleteEvent(int id)
	{
		var foundEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
		if (foundEvent is not null)
		{
			lock (_deletingEventLock)
			{
				foundEvent = _context.Events.FirstOrDefault(e => e.EventId == id);
				if (foundEvent is not null)
				{
					_context.Events.Remove(foundEvent);
					_context.SaveChanges();
					return true;
				}
			}
		}
		return false;
	}

	public async Task<List<Event>> GetEventList(int page, int countPerPage)
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
		return await _context.Events
			.OrderBy(e => e.DateTime)
			.Skip(skip)
			.Take(countPerPage)
			.ToListAsync();
	}
}
