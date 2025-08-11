using AutoMapper;
using CourseTracker.Courses.Dtos;
using CourseTracker.Entities;
using CourseTracker.Learners_.Dtos;
using CourseTracker.Modules.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // Course
            CreateMap<CourseDTO, CourseTracker.Entities.Course>();
            CreateMap<CreateUpdateCourseDTO, CourseTracker.Entities.Course>();

            // Learner
            CreateMap<Learner, LearnerDto>();
            CreateMap<Learners_.Dtos.CreateUpdateLearnerDTO, Learner>();

            // Module
            CreateMap<Module, ModuleDTO>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
                .ReverseMap();
            CreateMap<CreateUpdateModuleDTO, Module>();

            // Enrollment
            // CreateMap<EnrollmentDTO, Enrollment>();
            // CreateMap<CreateUpdateEnrollmentDTO, Enrollment>();

        }

       
    }
}
