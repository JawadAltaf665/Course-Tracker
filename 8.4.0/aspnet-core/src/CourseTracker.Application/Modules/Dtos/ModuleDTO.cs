using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseTracker.Entities;
using CourseTracker.Courses.Dtos;

namespace CourseTracker.Modules.Dtos
{
    public class ModuleDTO: EntityDto<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; } 
        public CourseDTO Course { get; set; } // Navigation property to the Course entity
    }
}
