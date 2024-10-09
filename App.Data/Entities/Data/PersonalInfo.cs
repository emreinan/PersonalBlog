﻿
namespace App.Data.Entities;

public class PersonalInfo : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string About { get; set; }
}