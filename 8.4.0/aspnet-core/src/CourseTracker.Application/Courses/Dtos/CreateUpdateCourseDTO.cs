using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Courses.Dtos
{
    public class CreateUpdateCourseDTO : EntityDto<int>
    {
        [Required(ErrorMessage = "Title is required"), StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
