using Microsoft.EntityFrameworkCore;
using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;

namespace SpokaneChildren.Api.Services;

public class ResourceService
{
	private readonly AppDbContext _context;
	private static object _deletingResourceLock = new();
	private static object _postingResourceLock = new();

	public ResourceService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<Resource> PostResource(ResourceDto dto)
	{
		if (dto.Name is null)
		{
			throw new ArgumentNullException(nameof(dto.Name));
		}
		if (dto.Name.Trim().Length == 0)
		{
			throw new ArgumentException($"{nameof(dto.Name)} is not allowed to be empty.");
		}

		Resource? foundResource = null;
		if (dto.ResourceId != -1)
		{
			foundResource = await _context.Resources.FirstOrDefaultAsync(resource => resource.ResourceId == dto.ResourceId);
		}
		if (foundResource is null)
		{
			lock (_postingResourceLock)
			{
				if (dto.ResourceId != -1)
				{
					foundResource = _context.Resources
						.FirstOrDefault(resource => resource.ResourceId == dto.ResourceId);
				}

				if (foundResource is null)
				{
					Resource addedResource = new()
					{
						Name = dto.Name,
						Category = dto.Category,
						Website = dto.Website,
						Phone = dto.Phone,
						Address = dto.Address,
					};
					_context.Add(addedResource);
					_context.SaveChanges();
					return addedResource;
				}
				else
				{
					foundResource.Name = dto.Name;
					foundResource.Category = dto.Category;
					foundResource.Website = dto.Website;
					foundResource.Phone = dto.Phone;
					foundResource.Address = dto.Address;
					_context.SaveChanges();
					return foundResource;
				}
			}
		}
		else
		{
			lock (_postingResourceLock)
			{
				foundResource.Name = dto.Name;
				foundResource.Category = dto.Category;
				foundResource.Website = dto.Website;
				foundResource.Phone = dto.Phone;
				foundResource.Address = dto.Address;
				_context.SaveChanges();
				return foundResource;
			}
		}
	}

	public async Task<bool> DeleteResource(int id)
	{
		var foundResource = await _context.Resources.FirstOrDefaultAsync(resource => resource.ResourceId == id);
		if (foundResource is not null)
		{
			lock (_deletingResourceLock)
			{
				foundResource = _context.Resources.FirstOrDefault(resource => resource.ResourceId == id);
				if (foundResource is not null)
				{
					_context.Resources.Remove(foundResource);
					_context.SaveChanges();
					return true;
				}
			}
		}
		return false;
	}

	// When you have better idea of categories and implementation,
	// Either have a dto that specifies category or group by category :)
	public async Task<List<IGrouping<ResourceCategory?, Resource>>> GetResourceList()
	{
		return await _context.Resources
			.GroupBy(resource => resource.Category)
			.ToListAsync();
		//var result = new List<List<Resource>>();
		//foreach (var grouping in list)
		//{
		//	var groupingList = new List<Resource>();
		//	foreach (var resource in grouping)
		//	{
		//		groupingList.Add(resource);
		//	}
		//	result.Add(groupingList);
		//}
		//return result;
	}
}
