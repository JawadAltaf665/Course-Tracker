using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Learners_.Dtos
{
    public class CreateUpdateLearnerDTO: EntityDto<int> 
    {

        [Required(ErrorMessage = "Name is required"), StringLength(100)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Email is required"), StringLength(100), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
