﻿
using App.Data.Entities.Auth;

namespace App.Data.Entities;

public class Education : Entity
{
    public string School { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid UserId { get; set; }

    public virtual User User { get; set; } = default!;

}