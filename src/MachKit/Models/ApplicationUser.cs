using Microsoft.AspNetCore.Identity;

namespace MachKit.Models;

public class ApplicationUser : IdentityUser
{
    public string? HoTen { get; set; }
}
