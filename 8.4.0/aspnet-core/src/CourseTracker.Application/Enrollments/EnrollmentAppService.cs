using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using CourseTracker.Enrollments.Dtos;
using CourseTracker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseTracker.Authorization.CourseTrackerAuthorizationProvider;

namespace CourseTracker.Enrollments
{
    public class EnrollmentAppService: ApplicationService, IEnrollmentAppService
    {
        public readonly IRepository<Enrollment, int> _enrollmentRepository;
        private readonly IMapper _mapper;

        public EnrollmentAppService(IRepository<Enrollment, int> enrollmentRepository, IMapper mapper)
        {
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
        }

        [AbpAuthorize(EnrollmentPermissions.Enrollments_Create)]
        public async Task CreateEnrollmentAsync(CreateUpdateEnrollmentDTO input)
        {
            var enrollment = new Enrollment
            {
                CourseId = input.CourseId,
                LearnerId = input.LearnerId,
                CompletionPercentage = input.CompletionPercentage,
                IsCompleted = input.IsCompleted
            };

            _mapper.Map<EnrollmentDTO>(enrollment);

            if (enrollment.CompletionPercentage >= 100)
            {
                enrollment.IsCompleted = true;
            }
            else
            {
                enrollment.IsCompleted = false;
            }

            await _enrollmentRepository.InsertAsync(enrollment);


        }

        [AbpAuthorize(EnrollmentPermissions.Enrollments_Delete)]
        public async Task DeleteEnrollmentAsync(int id)
        {
            var selectedEnrollment = _enrollmentRepository.FirstOrDefaultAsync(id);
            if (selectedEnrollment == null)
            {
                throw new UserFriendlyException($"Enrollment with ID {id} not found.");
            }

            await _enrollmentRepository.DeleteAsync(selectedEnrollment.Result);
        }

        public async Task<List<EnrollmentDTO>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _enrollmentRepository
                .GetAllIncluding(e => e.Course, e => e.Learner)
                .ToListAsync();

            if(!enrollments.Any())
            {
                throw new UserFriendlyException("No enrollments found.");
            }

            return _mapper.Map<List<EnrollmentDTO>>(enrollments);
        }

        public async Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _enrollmentRepository
                .GetAllIncluding(e => e.Course, e => e.Learner)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
            {
                throw new UserFriendlyException($"Enrollment with ID {id} not found.");
            }

            var enrollmentDto = _mapper.Map<EnrollmentDTO>(enrollment);

            return enrollmentDto;
        }

        [AbpAuthorize(EnrollmentPermissions.Enrollments_Update)]
        public async Task UpdateEnrollmentAsync(CreateUpdateEnrollmentDTO input)
        {
            var enrollment = await _enrollmentRepository.FirstOrDefaultAsync(input.Id);
            if (enrollment == null)
            {
                throw new UserFriendlyException($"Enrollment with ID {input.Id} not found.");
            }
            enrollment.CourseId = input.CourseId;
            enrollment.LearnerId = input.LearnerId;
            enrollment.CompletionPercentage = input.CompletionPercentage;
            enrollment.IsCompleted = input.IsCompleted;

            _mapper.Map<EnrollmentDTO>(enrollment);

            if (enrollment.CompletionPercentage >= 100)
            {
                enrollment.IsCompleted = true;
            }
            else
            {
                enrollment.IsCompleted = false; 
            }

            await _enrollmentRepository.UpdateAsync(enrollment);
        }

        public async Task<List<EnrollmentDTO>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            var enrollments = await _enrollmentRepository
                 .GetAllIncluding(e => e.Course, e => e.Learner)
                 .Where(e => e.CourseId == courseId)
                 .ToListAsync();

            if (!enrollments.Any())
            {
                throw new UserFriendlyException($"No enrollments found for Course ID {courseId}.");
            }

            return _mapper.Map<List<EnrollmentDTO>>(enrollments);
        }

        public async Task<List<EnrollmentDTO>> GetEnrollmentsByLearnerIdAsync(int learnerId)
        {
            var enrollments = await _enrollmentRepository
                .GetAllIncluding(e => e.Course, e => e.Learner)
                .Where(e => e.LearnerId == learnerId)
                .ToListAsync();
            if(!enrollments.Any())
            {
                throw new UserFriendlyException($"No enrollments found for Learner ID {learnerId}.");
            }
            return _mapper.Map<List<EnrollmentDTO>>(enrollments);
        }

    }
}
