using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using AutoMapper;
using CourseTracker.Courses.Dtos;
using CourseTracker.Enrollments.BackgroundJobServices;
using CourseTracker.Enrollments.Dtos;
using CourseTracker.Entities;
using CourseTracker.Learners_.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseTracker.Authorization.CourseTrackerAuthorizationProvider;

namespace CourseTracker.Enrollments
{
    public class EnrollmentAppService : ApplicationService, IEnrollmentAppService
    {
        public readonly IRepository<Enrollment, int> _enrollmentRepository;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public EnrollmentAppService(
            IRepository<Enrollment, int> enrollmentRepository,
            IMapper mapper,
            IBackgroundJobManager backgroundJob
            )
        {
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
            _backgroundJobManager = backgroundJob;
        }

        [AbpAuthorize(EnrollmentPermissions.Enrollments_Create)]
        public async Task CreateEnrollmentAsync(CreateUpdateEnrollmentDTO input)
        {
            var enrollment = new Enrollment
            {
                CourseId = input.CourseId,
                LearnerId = input.LearnerId,
                CompletionPercentage = input.CompletionPercentage,
                IsCompleted = input.CompletionPercentage >= 100
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

            var enrollmentId = await _enrollmentRepository.InsertAndGetIdAsync(enrollment);

            var savedEnrollment = await _enrollmentRepository
                .GetAll()
                .Include(e => e.Learner)
                .Include(e => e.Course)
                //.GetAllIncluding(e => e.Learner, e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (!savedEnrollment.IsCompleted)
            {
                await _backgroundJobManager.EnqueueAsync<EnrollmentCompletedEmailJob, EnrollmentCompletedEmailJobArgs>(
                  new EnrollmentCompletedEmailJobArgs
                  {
                      LearnerEmail = savedEnrollment.Learner.Email,
                      LearnerName = savedEnrollment.Learner.Name,
                      CourseTitle = savedEnrollment.Course.Title,
                      IsCompleted = savedEnrollment.IsCompleted,
                      CompletionPercentage = savedEnrollment.CompletionPercentage
                  });
            }
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

            if (!enrollments.Any())
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
            var enrollment = await _enrollmentRepository
                .GetAllIncluding(e => e.Course, e => e.Learner)
                .FirstOrDefaultAsync(e => e.Id == input.Id);
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

            // ✅ Trigger job only if 100% completed
            if (enrollment.IsCompleted == true && enrollment.CompletionPercentage == 100)
            {
                await _backgroundJobManager.EnqueueAsync<EnrollmentCompletedEmailJob, EnrollmentCompletedEmailJobArgs>(
                    new EnrollmentCompletedEmailJobArgs
                    {
                        LearnerEmail = enrollment.Learner.Email,
                        LearnerName = enrollment.Learner.Name,
                        CourseTitle = enrollment.Course.Title,
                        IsCompleted = enrollment.IsCompleted,
                        CompletionPercentage = enrollment.CompletionPercentage
                    }
                );
            }
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
            if (!enrollments.Any())
            {
                throw new UserFriendlyException($"No enrollments found for Learner ID {learnerId}.");
            }
            return _mapper.Map<List<EnrollmentDTO>>(enrollments);
        }

        public async Task<List<EnrollmentDTO>> GetEnrollmentsByKeyword(string keyword)
        {
            var enrollment = await _enrollmentRepository.GetAllIncluding(e => e.Course, e => e.Learner)
                .Where(
                e => e.Course.Title.Contains(keyword) || 
                e.Learner.Name.Contains(keyword) || 
                e.IsCompleted.ToString().Contains(keyword))
                .Select(e => new EnrollmentDTO
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    LearnerId = e.LearnerId,
                    CompletionPercentage = e.CompletionPercentage,
                    IsCompleted = e.IsCompleted,
                    CourseTitle = e.Course.Title,
                    LearnerName = e.Learner.Name
                }).ToListAsync();

            if (!enrollment.Any())
            {
                throw new UserFriendlyException($"No courses found with keyword '{keyword}'.");
            }

            return _mapper.Map<List<EnrollmentDTO>>(enrollment);

        }

        public async Task<PagedResultDto<EnrollmentDTO>> GetPagedEnrollmentsAsync(GetEnrollmentListInputDTO input)
        {
            var query = _enrollmentRepository
                .GetAllIncluding(e => e.Course, e => e.Learner);

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(e => e.Course.Title.Contains(input.Keyword)
                                      || e.Course.Description.Contains(input.Keyword)
                                      || e.Learner.Name.Contains(input.Keyword));
            }

            var totalCount = await query.CountAsync();

            var enrollments = await query
                .OrderBy(e => e.Course.Title) // Or you can order by enrollment date if you have that field
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToListAsync();

            var enrollmentDtos = _mapper.Map<List<EnrollmentDTO>>(enrollments);

            return new PagedResultDto<EnrollmentDTO>(totalCount, enrollmentDtos);
        }


    }
}
