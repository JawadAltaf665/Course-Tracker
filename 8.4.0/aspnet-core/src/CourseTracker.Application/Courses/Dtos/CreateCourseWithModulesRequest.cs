using CourseTracker.Modules.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Courses.Dtos
{
    public class CreateCourseWithModulesRequest
    {
        public CreateUpdateCourseDTO Course { get; set; }
        public List<CreateUpdateModuleDTO> Modules { get; set; }
    }
}
