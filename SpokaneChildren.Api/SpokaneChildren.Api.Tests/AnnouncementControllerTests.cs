﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class AnnouncementControllerTests
{
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = TestClient.GetTestClient();
	}

	[TestMethod]
	public async Task AddAnnouncement_NormalConditions_StatusCodeOk()
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = "Welcome to website!",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/announcement/addAnnouncement", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task AddAnnouncement_NormalConditions_ContentNotNull()
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = "Welcome to website!",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/announcement/addAnnouncement", jsonContent);
		var announcement = await response.Content.ReadFromJsonAsync<Announcement>();

		// Assert
		Assert.IsNotNull(announcement);
	}

	[TestMethod]
	[DataRow(null)]
	[DataRow("")]
	public async Task AddAnnouncement_NullOrEmptyTitle_StatusCodeBadRequest(string title)
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = title,
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/announcement/addAnnouncement", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteAnnouncement_ValidId_StatusCodeOk()
	{
		// Arrange
		var announcement = await AddAnnouncement();

		// Act
		var response = await _httpClient.PostAsync($"/announcement/deleteAnnouncement/{announcement.Id}", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteAnnouncement_InvalidId_StatusCodeOk()
	{
		// Arrange

		// Act
		var response = await _httpClient.PostAsync($"/announcement/deleteAnnouncement/-1", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	private async Task<Announcement> AddAnnouncement()
	{
		// Arrange
		AnnouncementDto dto = new()
		{
			Title = "Welcome to the website!",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/announcement/addAnnouncement", jsonContent);
		var announcement = await response.Content.ReadFromJsonAsync<Announcement>();

		// Assert
		Assert.IsNotNull(announcement);
		return announcement;
	}

	[TestMethod]
	public async Task GetAnnouncementList_NormalConditions_ReturnsList()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/announcement/getAnnouncementList?page=0");
		var content = await response.Content.ReadFromJsonAsync<List<Announcement>>();

		// Assert
		Assert.IsNotNull(content);
	}

	[TestMethod]
	public async Task GetAnnouncementList_NormalConditions_StatusCodeOk()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/announcement/getAnnouncementList?page=0");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	[DataRow(5, 0)]
	[DataRow(-1, 5)]
	public async Task GetAnnouncementList_BadArguments_StatusCodeBadRequest(int page, int countPerPage)
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync($"/announcement/getAnnouncementList?page={page}&countPerPage={countPerPage}");

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	public async Task GetAnnouncement_ValidId_StatusCodeOk()
	{
		// Arrange
		var announcement = await AddAnnouncement();

		// Act
		var response = await _httpClient.GetAsync($"/announcement/getAnnouncement?id={announcement.Id}");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task GetAnnouncement_ValidId_Success()
	{
		// Arrange
		var announcement = await AddAnnouncement();

		// Act
		var response = await _httpClient.GetAsync($"/announcement/getAnnouncement?id={announcement.Id}");
		var content = await response.Content.ReadFromJsonAsync<Announcement>();

		// Assert
		Assert.AreEqual(announcement.Id, content?.Id);
		Assert.AreEqual(announcement.Title, content?.Title);
		Assert.AreEqual(announcement.Description, content?.Description);
		Assert.AreEqual(announcement.DatePosted, content?.DatePosted);

	}

	[TestMethod]
	public async Task GetAnnouncement_InvalidId_StatusCodeBadRequest()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/announcement/getAnnouncement?id=-1");

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}
}
