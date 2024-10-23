﻿using App.Data.Entities.Auth;

namespace App.Data.Entities.Data;

public class Education : Entity<int>
{
    public string School { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}