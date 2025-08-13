using Abp.Application.Services;
using CourseTracker.Courses.Dtos;
using CourseTracker.Modules.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Course
{
    public interface ICourseAppService : IApplicationService
    {
        Task<List<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDTO> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(CreateUpdateCourseDTO input);
        Task UpdateCourseAsync(CreateUpdateCourseDTO input);
        Task DeleteCourseAsync(int id);
        Task<List<CourseDTO>> GetCoursesByKeyword(string keyword);
        Task CreateCourseWIthModuleDTO(CreateCourseWithModulesRequest input);
    }
}
