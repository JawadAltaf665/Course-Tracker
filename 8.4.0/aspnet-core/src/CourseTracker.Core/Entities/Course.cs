using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Entities
{
    public class Course: FullAuditedAggregateRoot<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
