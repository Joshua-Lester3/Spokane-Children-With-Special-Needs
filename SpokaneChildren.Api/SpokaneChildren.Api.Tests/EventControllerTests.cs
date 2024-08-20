using Microsoft.AspNetCore.Mvc.Testing;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
using System.Net.Http.Json;
using System.Net;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class EventControllerTests
{
	private static readonly WebApplicationFactory<Program> _factory = new();
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = _factory.CreateClient();
	}

	[TestMethod]
	public async Task AddEvent_NormalConditions_StatusCodeOk()
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			DateTime = DateTime.UtcNow,
			Location = "East side park",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/event/addEvent", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task AddEvent_NormalConditions_ContentNotNull()
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			DateTime = DateTime.UtcNow,
			Location = "East side park",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/event/addEvent", jsonContent);
		var e = await response.Content.ReadFromJsonAsync<Event>();

		// Assert
		Assert.IsNotNull(e);
	}

	[TestMethod]
	[DataRow("   ", "Fun event", "East side park")]
	[DataRow("Test Event :)", "    ", "East side park")]
	[DataRow("Test Event :)", "Fun event", "    ")]
	[DataRow("", "Fun event", "East side park")]
	[DataRow("Test Event :)", "", "East side park")]
	[DataRow("Test Event :)", "Fun event", "")]
	[DataRow(null, "Fun event", "East side park")]
	[DataRow("Test Event :)", null, "East side park")]
	[DataRow("Test Event :)", "Fun event", null)]
	public async Task AddEvent_NullOrEmptyField_StatusCodeBadRequest(string eventName, string description, string location)
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = eventName,
			Description = description,
			DateTime = DateTime.UtcNow,
			Location = location,
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/event/addEvent", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteEvent_ValidId_StatusCodeOk()
	{
		// Arrange
		var e = await AddEvent();

		// Act
		var response = await _httpClient.PostAsync($"/event/deleteEvent/{e.EventId}", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteEvent_InvalidId_StatusCodeBadRequest()
	{
		// Arrange

		// Act
		var response = await _httpClient.PostAsync($"/event/deleteEvent/-1", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	private async Task<Event> AddEvent()
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			DateTime = DateTime.UtcNow,
			Location = "East side park",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/event/addEvent", jsonContent);
		var e = await response.Content.ReadFromJsonAsync<Event>();

		// Assert
		Assert.IsNotNull(e);
		return e;
	}

	[TestMethod]
	public async Task GetEventList_NormalConditions_ReturnsList()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/event/getEventList?page=0");
		var content = await response.Content.ReadFromJsonAsync<List<Event>>();

		// Assert
		Assert.IsNotNull(content);
	}

	[TestMethod]
	public async Task GetEventList_NormalConditions_StatusCodeOk()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/event/getEventList?page=0");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	[DataRow(5, 0)]
	[DataRow(-1, 5)]
	public async Task GetEventList_BadArguments_StatusCodeBadRequest(int page, int countPerPage)
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync($"/event/getEventList?page={page}&countPerPage={countPerPage}");

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}
}
