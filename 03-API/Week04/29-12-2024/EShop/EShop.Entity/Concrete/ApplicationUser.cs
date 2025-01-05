using System;
using EShop.Shared.ComplexTypes;
using Microsoft.AspNetCore.Identity;

namespace EShop.Entity.Concrete;

public class ApplicationUser : IdentityUser //IdentityUser sınıfını miras alır. IdentityUser, kullanıcı bilgilerini tutar.
{
    private ApplicationUser()
    {

    }
    public ApplicationUser(string firstName, string lastName, DateTime dateOfBirty, GenderType gender)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirty = dateOfBirty;
        Gender = gender;
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public DateTime DateOfBirty { get; set; }
    public GenderType Gender { get; set; }
}
