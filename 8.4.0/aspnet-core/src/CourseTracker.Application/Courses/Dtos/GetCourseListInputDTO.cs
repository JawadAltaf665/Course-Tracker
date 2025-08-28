using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Courses.Dtos
{
    public class GetCourseListInputDTO: PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
