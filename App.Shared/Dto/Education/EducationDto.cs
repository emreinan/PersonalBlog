﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.Education
{
    public class EducationDto
    {
        public string School { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class EducationDtoValidator : AbstractValidator<EducationDto>
    {
        public EducationDtoValidator()
        {

            RuleFor(x => x.School).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Degree).NotEmpty().MaximumLength(100);
            RuleFor(x => x.FieldOfStudy).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartDate).NotEmpty();
        }
    }
}