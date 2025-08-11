using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Modules.Dtos
{
    public class CreateUpdateModuleDTO: EntityDto<int>
    {
        [Required(ErrorMessage = "Title is required."), StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }
        public int CourseId { get; set; } 
    }
}
