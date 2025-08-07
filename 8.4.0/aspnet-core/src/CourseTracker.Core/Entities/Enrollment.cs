using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Entities
{
    public class Enrollment : FullAuditedAggregateRoot<int>
    {
        public int LearnerId { get; set; }
        public Learner Learner { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public double CompletionPercentage { get; set; } 
        public bool IsCompleted { get; set; }
    }
}
