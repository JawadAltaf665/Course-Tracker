using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Entities
{
    public class Module: FullAuditedAggregateRoot<int>
    {
        public string Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
