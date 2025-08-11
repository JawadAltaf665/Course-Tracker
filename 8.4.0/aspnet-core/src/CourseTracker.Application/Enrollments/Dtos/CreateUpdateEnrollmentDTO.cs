using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Enrollments.Dtos
{
    public class CreateUpdateEnrollmentDTO: EntityDto<int>
    {
        public int CourseId { get; set; }
        public int LearnerId { get; set; }
        public double CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
        
    }
}
