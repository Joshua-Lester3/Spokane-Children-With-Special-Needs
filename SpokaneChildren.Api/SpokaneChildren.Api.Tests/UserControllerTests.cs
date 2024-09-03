using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Identity;
using SpokaneChildren.Api.Models;
using System.Net.Http.Json;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class UserControllerTests
{
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = TestClient.GetTestClient();
	}

	[TestMethod]
	public async Task AddUser_ValidDto_Success()
	{

	}

	private async Task<AppUser> AddUser()
	{
		// Arrange
		UserDto dto = new UserDto
		{
			Username = "Jimbob",
			Email = "jimbob@gmail.com",
			Password = "FlippantFlippersFlipBurgers",
			Role = Roles.Moderator
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/user/addUser", jsonContent);
		var user = await response.Content.ReadFromJsonAsync<AppUser>();

		// Assert
		Assert.IsNotNull(user);
		return user;
	}

	//[TestMethod]
	//public async Task GetUser_ValidId_Success()
	//{

	//}
}