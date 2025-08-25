using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using AutoMapper;
using CourseTracker.Course;
using CourseTracker.Courses;
using CourseTracker.Courses.Dtos;
using CourseTracker.Entities;
using CourseTracker.Modules.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseTracker.Authorization.CourseTrackerAuthorizationProvider;

namespace CourseTracker.AppService
{
    public class CourseAppService: ApplicationService, ICourseAppService
    {
        private readonly IRepository<Entities.Course, int> _courseRepo;
        private readonly IRepository<Module, int> _moduleRepo;
        private readonly IMapper _mapper; 


        public CourseAppService(
            IRepository<Entities.Course, int> courseRepo,
            IMapper mapper,
            IRepository<Module, int> moduleRepo)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
            _moduleRepo = moduleRepo;
        }

        [AbpAuthorize(CoursePermissions.Courses_Create)]
        public async Task CreateCourseAsync(CreateUpdateCourseDTO input)
        {
            if(string.IsNullOrWhiteSpace(input.Title))
            {
                throw new UserFriendlyException("Course title cannot be empty.");
            }

            var course = new Entities.Course
            {
                Title = input.Title,
                Description = input.Description
            };

            _mapper.Map<CourseTracker.Entities.Course>(course);

            await _courseRepo.InsertAsync(course);

        }

        [AbpAuthorize(CoursePermissions.Courses_Delete)]
        public async Task DeleteCourseAsync(int id)
        {
            var selectedCourse = await _courseRepo.FirstOrDefaultAsync(id);

            if (selectedCourse == null)
            {
                throw new UserFriendlyException($"Course with ID {id} not found.");
            }

            await _courseRepo.DeleteAsync(selectedCourse);
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepo.FirstOrDefaultAsync(id);

            if (course == null)
            {
                throw new Exception($"Course with ID {id} not found.");
            }
            var courseDto = _mapper.Map<CourseDTO>(course);

            return courseDto;

            //var courseDto = new CourseDTO
            //{
            //    Id = course.Id,
            //    Title = course.Title,
            //    Description = course.Description
            //};
            //return courseDto;
        }


        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllListAsync();

            if (!courses.Any()) {
                throw new UserFriendlyException("No courses found.");
            }

            return _mapper.Map<List<CourseDTO>>(courses);
        }

        [AbpAuthorize(CoursePermissions.Courses_Update)]
        public async Task UpdateCourseAsync(CreateUpdateCourseDTO input)
        {
            var selectedCourse = await _courseRepo.FirstOrDefaultAsync(input.Id);
            if (selectedCourse == null)
            {
                throw new Exception($"Course with ID {input.Id} not found.");
            }
            selectedCourse.Title = input.Title;
            selectedCourse.Description = input.Description;

            await _courseRepo.UpdateAsync(selectedCourse);

        }

        [UnitOfWork] [RemoteService(false)]
        public async Task CreateCourseWIthModuleDTO(CreateCourseWithModulesRequest input)
        {
            var course = _mapper.Map<Entities.Course>(input.Course);
            await _courseRepo.InsertAsync(course);
            var courseId = await _courseRepo.InsertAndGetIdAsync(course);

            foreach (var moduleDto in input.Modules)
            {
                var module = _mapper.Map<Entities.Module>(moduleDto);
                module.CourseId = course.Id;
                await _moduleRepo.InsertAsync(module);
            }
        }

        public async Task<List<CourseDTO>> GetCoursesByKeyword(string keyword)
        {
            var course = await _courseRepo.GetAll()
                .Where(c => c.Title.Contains(keyword) || c.Description.Contains(keyword))
                .Select(c => new CourseDTO
                {
                    Title = c.Title,
                    Description = c.Description,
                    Id = c.Id
                }).ToListAsync();

            return _mapper.Map<List<CourseDTO>>(course);
            
        }
    }
}
