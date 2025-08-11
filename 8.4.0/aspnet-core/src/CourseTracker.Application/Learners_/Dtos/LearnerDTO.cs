using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Learners_.Dtos
{
    public class LearnerDto: EntityDto<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
