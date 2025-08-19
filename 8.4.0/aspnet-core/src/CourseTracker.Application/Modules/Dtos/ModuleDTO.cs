using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseTracker.Entities;
using CourseTracker.Courses.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CourseTracker.Modules.Dtos
{
    public class ModuleDTO: EntityDto<int>
    {
        [Required, StringLength(100, ErrorMessage = "Title cannot exceed 100 characters."), Display(Name = "Module Title")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Course ID is required."), Display(Name = "Course")]
        public int CourseId { get; set; } 
        public CourseDTO Course { get; set; } // Navigation property to the Course entity
    }
}
