using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using CourseTracker.Course;
using CourseTracker.Courses;
using CourseTracker.Courses.Dtos;
using CourseTracker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.AppService
{
    public class CourseAppService: ApplicationService, ICourseAppService
    {
        private readonly IRepository<CourseTracker.Entities.Course, int> _courseRepo;
        private readonly IMapper _mapper; 


        public CourseAppService(IRepository<CourseTracker.Entities.Course, int> courseRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;

        }

        public async Task CreateCourseAsync(CreateUpdateCourseDTO input)
        {
            if(string.IsNullOrWhiteSpace(input.Title))
            {
                throw new UserFriendlyException("Course title cannot be empty.");
            }

            var course = new CourseTracker.Entities.Course
            {
                Title = input.Title,
                Description = input.Description
            };
            _mapper.Map<CourseTracker.Entities.Course>(course);

            await _courseRepo.InsertAsync(course);

        }

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

            var allCourses = courses.Select(courses =>
                new CourseDTO
                {
                    Id = courses.Id,
                    Title = courses.Title,
                    Description = courses.Description,
                }).ToList();

            return allCourses;
        }


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

        public Task<List<CourseDTO>> GetCoursesByKeyword(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
