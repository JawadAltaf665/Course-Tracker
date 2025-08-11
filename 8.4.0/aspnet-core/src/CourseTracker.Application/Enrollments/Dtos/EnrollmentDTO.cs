using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Enrollments.Dtos
{
    public class EnrollmentDTO : EntityDto<int>
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }

        public int LearnerId { get; set; }
        public string LearnerName { get; set; }

        public double CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
    }

}

