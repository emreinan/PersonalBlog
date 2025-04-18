﻿
namespace App.Shared.Dto.Project;

public class ProjectDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsActive { get; set; }

}
