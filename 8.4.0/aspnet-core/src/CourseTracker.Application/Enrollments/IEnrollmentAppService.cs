using Abp.Application.Services;
using CourseTracker.Enrollments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Enrollments
{
    public interface IEnrollmentAppService: IApplicationService
    {
        Task<List<EnrollmentDTO>> GetAllEnrollmentsAsync();
        Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id);
        Task CreateEnrollmentAsync(CreateUpdateEnrollmentDTO input);
        Task UpdateEnrollmentAsync(CreateUpdateEnrollmentDTO input);
        Task DeleteEnrollmentAsync(int id);
        Task<List<EnrollmentDTO>> GetEnrollmentsByLearnerIdAsync(int learnerId);
        Task<List<EnrollmentDTO>> GetEnrollmentsByCourseIdAsync(int courseId);
        Task<List<EnrollmentDTO>> GetEnrollmentsByKeyword(string keyword);
    }
}
