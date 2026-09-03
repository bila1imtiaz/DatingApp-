using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; }="";
    [Required]
    [EmailAddress]
    public string Email { get; set; }="";
    [Required]
    [StringLength(12, MinimumLength = 6)]
    public string Password { get; set; }="";
    
}
