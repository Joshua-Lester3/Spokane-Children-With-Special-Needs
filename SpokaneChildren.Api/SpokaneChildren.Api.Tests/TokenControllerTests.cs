using Microsoft.AspNetCore.Mvc.Testing;
using SpokaneChildren.Api.Dtos;
using System.Net.Http.Json;
using System.Net;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class TokenControllerTests
{
	private static readonly WebApplicationFactory<Program> _factory = new();
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = _factory.CreateClient();
	}

	[TestMethod]
	public async Task GetToken_CorrectUsernameAndPassword_StatusCodeIsOK()
	{
		// Arrange
		UserCredentialsDto dto = new()
		{
			Username = "Thors",
			Password = "Passw0rd&321",
		};
		var content = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/token/getToken", content);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task GetToken_IncorrectUsernameAndPassword_StatusCodeIsUnauthorized()
	{
		// Arrange
		UserCredentialsDto dto = new()
		{
			Username = "Thorfinn",
			Password = "WhatIsATrueWarrior",
		};
		var content = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/token/getToken", content);

		// Assert
		Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
	}

	[TestMethod]
	[DataRow("Thors", "")]
	[DataRow("", "Thorfinn")]
	[DataRow(null, "Thorfinn")]
	[DataRow("Thors", null)]
	public async Task GetToken_EmptyOrNullParameters_StatusCodeIsBad(string username, string password)
	{
		// Arrange
		UserCredentialsDto dto = new()
		{
			Username = username,
			Password = password,
		};
		var content = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/token/getToken", content);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}
}
