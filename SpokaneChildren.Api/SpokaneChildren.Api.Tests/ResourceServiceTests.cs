using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class ResourceServiceTests : DatabaseTestBase
{
	private ResourceService _service = null!;
	private AppDbContext _context = null!;

	[TestInitialize]
	public void Init()
	{
		_context = new AppDbContext(Options);
		_service = new ResourceService(_context);
	}

	[TestMethod]
	public async Task AddResource_NameIsPopulated_Success()
	{
		// Arrange
		var dto = new ResourceDto
		{
			Name = "Test Resource",
			Category = ResourceCategory.Therapy,
			Website = "Therapy.org"
		};

		// Act
		var response = await _service.PostResource(dto);

		// Assert
		CollectionAssert.Contains(_context.Resources.ToList(), response);
	}

	[TestMethod]
	public async Task AddResource_NameNull_ThrowsArgumentNullException()
	{
		// Arrange
		var dto = new ResourceDto
		{
			Name = null!,
			Category = ResourceCategory.Therapy,
			Website = "Therapy.org"
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _service.PostResource(dto));
	}

	[TestMethod]
	[DataRow("")]
	[DataRow("    ")]
	public async Task AddResource_EmptyName_ThrowsArgumentException(string name)
	{
		// Arrange
		var dto = new ResourceDto
		{
			Name = name,
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _service.PostResource(dto));
	}

	[TestMethod]
	public async Task PostResource_UpdateAddedResource_ChangesAreSaved()
	{
		// Arrange
		var dto = new ResourceDto
		{
			Name = "Test resource",
			Category = ResourceCategory.Therapy,
			Website = "Therapy.org",
			Phone = "509-444-3232",
			Address = "124 S West St",
		};
		var addedResponse = await _service.PostResource(dto);
		var editingDto = new ResourceDto
		{
			Name = "New test resource",
			Category = ResourceCategory.Psychiatrist,
			Website = "TherapyNew.org",
			Phone = "508-333-2121",
			Address = "123 N West St",
		};

		// Act
		var response = await _service.PostResource(editingDto);

		// Assert
		Assert.AreEqual(editingDto.Name, response.Name);
		Assert.AreEqual(editingDto.Website, response.Website);
		Assert.AreEqual(editingDto.Category, response.Category);
		Assert.AreEqual(editingDto.Phone, response.Phone);
		Assert.AreEqual(editingDto.Address, response.Address);

	}

	[TestMethod]
	public async Task DeleteResource_ValidId_RemovesResource()
	{
		// Arrange
		var resource = await AddResource();

		// Act
		var response = await _service.DeleteResource(resource.ResourceId);

		// Assert
		Assert.IsTrue(response);
		CollectionAssert.DoesNotContain(_context.Resources.ToList(), resource);
	}

	[TestMethod]
	public async Task DeleteResource_InvalidId_ReturnsFalse()
	{
		// Arrange

		// Act
		var response = await _service.DeleteResource(-1);

		// Assert
		Assert.IsFalse(response);
	}

	private async Task<Resource> AddResource(ResourceCategory category = ResourceCategory.Therapy)
	{
		var dto = new ResourceDto
		{
			Name = "Test resource",
			Category = category,
			Website = "Therapy.org",
			Phone = "509-444-3232",
			Address = "124 S West St",
		};
		return await _service.PostResource(dto);
	}

	[TestMethod]
	public async Task GetResourceList_NormalConditions_ReturnsListOfResourcesByCategory()
	{
		// Arrange
		var resource1 = await AddResource();
		var resource2 = await AddResource(ResourceCategory.Psychiatrist);
		var expectedOrderedList = new Resource[][] { [resource1], [resource2] };

		// Act
		var resourceList = await _service.GetResourceList();

		// Assert
		CollectionAssert.AreEqual(expectedOrderedList[0], resourceList[0].ToList());
		CollectionAssert.AreEqual(expectedOrderedList[1], resourceList[1].ToList());
	}
}
