﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Identity;
using SpokaneChildren.Api.Models;

namespace SpokaneChildren.Api.Services;

public class UserService
{
	private readonly UserManager<AppUser> _userManager;

	public UserService(UserManager<AppUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<IdentityResultDto> AddUser(UserDto userDto)
	{
		if (await _userManager.FindByEmailAsync(userDto.Email) == null && await _userManager.FindByNameAsync(userDto.Username) == null)
		{
			AppUser user = new AppUser
			{
				Email = userDto.Email,
				UserName = userDto.Username
			};

			IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);

			if (result.Succeeded)
			{
				result = await _userManager.AddToRoleAsync(user, Roles.Admin);
				if (result.Succeeded)
				{
					return new IdentityResultDto() { Result = IdentityResultEnum.Success };
				}
				else
				{
					return new IdentityResultDto { Result = IdentityResultEnum.Failure, Errors = result.Errors };
				}
			}
			else
			{
				return new IdentityResultDto { Result = IdentityResultEnum.Failure, Errors = result.Errors };
			}

		}
		IdentityError error = new IdentityError();
		error.Description = "Account with this email or username already exists.";
		return new IdentityResultDto { Result = IdentityResultEnum.AccountExists, Errors = [ error ] };
	}

	public async Task<IdentityResultDto> UpdateUserInfo(UpdateUserInfoDto userDto)
	{
		var user = await _userManager.FindByIdAsync(userDto.Id);
		if (user is null)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.AccountDoesNotExist };
		}
		user.UserName = userDto.NewUsername;
		user.Email = userDto.NewEmail;
		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		var roles = await _userManager.GetRolesAsync(user);
		foreach (var role in roles)
		{
			result = await _userManager.RemoveFromRoleAsync(user, role);
			if (!result.Succeeded)
			{
				return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
			}
		}
		result = await _userManager.AddToRoleAsync(user, userDto.NewRole);
		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		return new IdentityResultDto() { Result = IdentityResultEnum.Success };
	}

	public async Task<IdentityResultDto> ChangePassword(ChangePasswordDto dto)
	{
		var user = await _userManager.FindByIdAsync(dto.Id);
		if (user is null)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.AccountDoesNotExist };
		}
		var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		return new IdentityResultDto() { Result = IdentityResultEnum.Success };
	}

	public async Task<IdentityResultDto> DeleteUser(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is null)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.AccountDoesNotExist };
		}
		var result = await _userManager.DeleteAsync(user);
		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		return new IdentityResultDto() { Result = IdentityResultEnum.Success };
	}

	public async Task<UserInfoDto?> GetUser(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is not null)
		{
			var roles = await _userManager.GetRolesAsync(user);
			var dto = new UserInfoDto
			{
				UserId = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Role = roles.Count() > 0 ? roles[0] : null,
			};
			return dto;
		}
		return null;
	}

	public async Task<List<UserInfoDto>> GetUserList()
	{
		var users = await _userManager.Users.ToListAsync();
		var result = new List<UserInfoDto>();
		foreach(var user in users)
		{
			var roles = await _userManager.GetRolesAsync(user);
			var dto = new UserInfoDto
			{
				UserId = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Role = roles.Count() > 0 ? roles[0] : null,
			};
			result.Add(dto);
		}
		return result;
	}
}

public class IdentityResultDto
{
	public IdentityResultEnum Result { get; set; }
	public IEnumerable<IdentityError>? Errors { get; set; }
}

public enum IdentityResultEnum
{
	Success,
	AccountExists,
	AccountDoesNotExist,
	Failure,
}
