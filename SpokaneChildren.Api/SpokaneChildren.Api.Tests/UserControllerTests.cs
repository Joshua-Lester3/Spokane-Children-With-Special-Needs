using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Identity;
using SpokaneChildren.Api.Models;
using System.Data;
using System.Net;
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
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	[DataRow("Thors")]
	public async Task AddUser_UsernameFieldEmptyNullOrExisting_BadRequest(string username)
	{
		// Arrange
		UserDto dto = new UserDto
		{
			Username = username,
			Email = "jimbob@gmail.com",
			Password = "FlippantFlippersFlipBurgers%1",
			Role = Roles.Moderator
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/user/addUser", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task AddUser_EmailFieldEmptyOrNull_BadRequest(string email)
	{
		// Arrange
		UserDto dto = new UserDto
		{
			Username = "Thorfinn",
			Email = email,
			Password = "FlippantFlippersFlipBurgers%1",
			Role = Roles.Moderator
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/user/addUser", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task AddUser_PasswordFieldEmptyOrNull_BadRequest(string password)
	{
		// Arrange
		UserDto dto = new UserDto
		{
			Username = "Thorfinn",
			Email = "Email@email.com",
			Password = password,
			Role = Roles.Moderator
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/user/addUser", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	[DataRow("NotARole")]
	public async Task AddUser_RolesFieldEmptyNullOrInvalid_BadRequest(string role)
	{
		// Arrange
		UserDto dto = new UserDto
		{
			Username = "Thorfinn",
			Email = "Email@email.com",
			Password = "password123455678890%",
			Role = role,
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/user/addUser", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task GetUser_IdEmptyOrNull_BadRequest(string id)
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync($"/user/getUser?id={id}");

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task ChangePassword_IdEmptyOrNull_BadRequest(string id)
	{
		// Arrange
		var dto = new ChangePasswordDto()
		{
			Id = id,
			CurrentPassword = "password",
			NewPassword = "newPassword"
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync($"/user/changePassword", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task ChangePassword_CurrentPasswordEmptyOrNull_BadRequest(string currentPassword)
	{
		// Arrange
		var dto = new ChangePasswordDto()
		{
			Id = "123",
			CurrentPassword = currentPassword,
			NewPassword = "newPassword"
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync($"/user/changePassword", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task ChangePassword_NewPasswordEmptyOrNull_BadRequest(string newPassword)
	{
		// Arrange
		var dto = new ChangePasswordDto()
		{
			Id = "123",
			CurrentPassword = "current",
			NewPassword = newPassword
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync($"/user/changePassword", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task UpdateUserInfo_IdEmptyOrNull_BadRequest(string id)
	{
		// Arrange
		var dto = new UpdateUserInfoDto()
		{
			Id = id,
			NewUsername = "username",
			NewRole = "Admin",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync($"/user/updateUserInfo", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	public async Task UpdateUserInfo_NewUsernameEmptyOrNull_BadRequest(string newUsername)
	{
		// Arrange
		var dto = new UpdateUserInfoDto()
		{
			Id = "123",
			NewUsername = newUsername,
			NewRole = "Admin",
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync($"/user/updateUserInfo", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("   ")]
	[DataRow("")]
	[DataRow(null)]
	[DataRow("NotARole")]
	public async Task UpdateUserInfo_NewRoleEmptyNullOrInvalid_BadRequest(string newRole)
	{
		// Arrange
		var dto = new UpdateUserInfoDto()
		{
			Id = "123",
			NewUsername = "newUsername",
			NewRole = newRole,
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync($"/user/updateUserInfo", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	public async Task GetUserList_Success()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync($"/user/getUserList");
		var content = await response.Content.ReadFromJsonAsync<List<UserInfoDto>>();

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		Assert.IsNotNull(content);
	}

	// TODO: Find solution for testing UserController with non-duplicate emails/usernames

	// I'm not sure I can test with actually adding/deleting/updating a user
	// because duplicate emails/usernames aren't allowed. So I'm scrapping
	// these tests, and others involving actually adding/deleting/updating until
	// I find a good solution.

	//private async Task<AppUser> AddUser()
	//{
	//	// Arrange
	//	UserDto dto = new UserDto
	//	{
	//		Username = "Jimbob",
	//		Email = "jimbob@gmail.com",
	//		Password = "FlippantFlippersFlipBurgers%1",
	//		Role = Roles.Moderator
	//	};
	//	var jsonContent = JsonContent.Create(dto);

	//	// Act
	//	var response = await _httpClient.PostAsync("/user/addUser", jsonContent);
	//	var user = await response.Content.ReadFromJsonAsync<AppUser>();

	//	// Assert
	//	Assert.IsNotNull(user);
	//	return user;
	//}

	//[TestMethod]
	//public async Task GetUser_ValidId_Success()
	//{
	//	// Arrange
	//	var user = await AddUser();

	//	// Act
	//	var response = await _httpClient.GetAsync($"/user/getUser?id={user.Id}");
	//	var userResult = await response.Content.ReadFromJsonAsync<AppUser>();

	//	// Assert
	//	Assert.IsNotNull(userResult);
	//	Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	//}

	//[TestMethod]
	//public async Task AddUser_ValidDto_Success()
	//{
	//	// Arrange
	//	UserDto dto = new UserDto
	//	{
	//		Username = "Jimbob",
	//		Email = "jimbob@gmail.com",
	//		Password = "FlippantFlippersFlipBurgers%1",
	//		Role = Roles.Moderator
	//	};
	//	var jsonContent = JsonContent.Create(dto);

	//	// Act
	//	var response = await _httpClient.PostAsync("/user/addUser", jsonContent);

	//	// Assert
	//	Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	//}
}