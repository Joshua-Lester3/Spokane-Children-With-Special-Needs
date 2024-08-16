

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class AnnouncementControllerTests
{
	private static readonly WebApplicationFactory<Program> _factory = new();
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = _factory.CreateClient();
	}

	[TestMethod]
	public async Task Test_Success()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/announcement/addAnnouncement");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}
}
