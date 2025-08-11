using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Entities
{
    public class Enrollment : FullAuditedEntity<int>
    {
        public int LearnerId { get; set; }

        [ForeignKey("LearnerId")]
        public Learner Learner { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public double CompletionPercentage { get; set; } 
        public bool IsCompleted { get; set; }
    }
}
