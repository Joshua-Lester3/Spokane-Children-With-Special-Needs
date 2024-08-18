using Microsoft.AspNetCore.Mvc.Testing;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
using System.Net.Http.Json;
using System.Net;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class ResourceControllerTests
{
	private static readonly WebApplicationFactory<Program> _factory = new();
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = _factory.CreateClient();
	}

	[TestMethod]
	public async Task AddResource_NormalConditions_StatusCodeOk()
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
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/resource/addResource", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task AddResource_NormalConditions_ContentNotNull()
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
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/resource/addResource", jsonContent);
		var resource = await response.Content.ReadFromJsonAsync<Resource>();

		// Assert
		Assert.IsNotNull(resource);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow(null)]
	[DataRow("")]
	public async Task AddResource_NullOrEmptyName_StatusCodeBadRequest(string name)
	{
		// Arrange
		var dto = new ResourceDto
		{
			Name = name,
			Category = ResourceCategory.Therapy,
			Website = "Therapy.org",
			Phone = "509-444-3232",
			Address = "124 S West St",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/resource/addResource", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteResource_ValidId_StatusCodeOk()
	{
		// Arrange
		var resource = await AddResource();

		// Act
		var response = await _httpClient.PostAsync($"/resource/deleteResource/{resource.ResourceId}", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteResource_InvalidId_StatusCodeOk()
	{
		// Arrange

		// Act
		var response = await _httpClient.PostAsync($"/resource/deleteResource/-1", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	private async Task<Resource> AddResource()
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
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/resource/addResource", jsonContent);
		var resource = await response.Content.ReadFromJsonAsync<Resource>();

		// Assert
		Assert.IsNotNull(resource);
		return resource;
	}

	[TestMethod]
	public async Task GetResourceList_NormalConditions_ReturnsList()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/resource/getResourceList");
		var content = await response.Content.ReadFromJsonAsync<List<Resource>>();

		// Assert
		Assert.IsNotNull(content);
	}

	[TestMethod]
	public async Task GetResourceList_NormalConditions_StatusCodeOk()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/resource/getResourceList");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}
}