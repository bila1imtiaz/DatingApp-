using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static class AppUserExtension
{
    public static UserDto toDo(this AppUser user,ITokenService tokenService)
    {
        return new UserDto
        {
          Id= user.Id,
          Email=user.Email,
          DisplayName=user.Username,
          Token= tokenService.CreateToken(user) 
        };
    }

}
