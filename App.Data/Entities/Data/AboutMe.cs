﻿namespace App.Data.Entities.Data;

public class AboutMe : Entity<int>
{
    public string Introduciton { get; set; }
    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }
}
