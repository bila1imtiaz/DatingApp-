using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;    
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Extensions;

namespace API.Controllers;

public class AccountController (AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
    {
      
        if (await EmailExists(registerDto.Email)) return BadRequest("Email is already taken");

        using var hmac= new HMACSHA512();

        var user= new AppUser
        {
            Email=registerDto.Email,
            Username=registerDto.DisplayName,
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt=hmac.Key
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.toDo(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x=> x.Email==loginDto.Email);

        if (user == null ) return Unauthorized("Invalid Email");   

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputeHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
    
        for (var i=0; i<ComputeHash.Length; i++)
        {
            if(ComputeHash[i]!= user.PasswordHash[i]) return Unauthorized("invalid password");
        }

        return new UserDto
        {
          Id= user.Id,
          Email=user.Email,
          DisplayName=user.Username,
          Token= tokenService.CreateToken(user) 
        };
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x=>x.Email.ToLower()== email.ToLower());
    }

} 


