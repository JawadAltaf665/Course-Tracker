using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
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

namespace CourseTracker.Modules
{
    public class ModuleAppService : ApplicationService, IModuleAppService
    {
        private readonly IRepository<Module, int> _moduleRepository;
        private readonly IMapper _mapper;

        public ModuleAppService(IRepository<Module, int> moduleRepository, IMapper mapper)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
        }

        [AbpAuthorize(ModulePermissions.Modules_Create)]
        public async Task CreateModuleAsync(CreateUpdateModuleDTO input)
        {
            var module = new Module
            {
                Title = input.Title,
                Description = input.Description,
                CourseId = input.CourseId
            };

            _mapper.Map<Module>(module);

            await _moduleRepository.InsertAsync(module);
        }


        [AbpAuthorize(ModulePermissions.Modules_Delete)]
        public async Task DeleteModuleAsync(int id)
        {
            var selectedModule = await _moduleRepository.FirstOrDefaultAsync(id);
            if (selectedModule == null)
            {
                throw new UserFriendlyException($"Module with ID {id} not found.");
            }

            await _moduleRepository.DeleteAsync(selectedModule);
        }

        public async Task<List<ModuleDTO>> GetAllModulesAsync()
       {
            // var modules = await _moduleRepository.GetAllListAsync();
            var modules = await _moduleRepository
                .GetAllIncluding(m => m.Course)
                .ToListAsync();

            if (!modules.Any()) {
                throw new UserFriendlyException("No modules found.");
            }

            return _mapper.Map<List<ModuleDTO>>(modules);

        }

        public async Task<ModuleDTO> GetModuleByIdAsync(int id)
        {
            var module = await _moduleRepository.FirstOrDefaultAsync(id);
            if (module == null)
            {
                throw new UserFriendlyException($"Module with ID {id} not found.");
            }

            var moduleDto = _mapper.Map<ModuleDTO>(module);

            return moduleDto;
        }

        [AbpAuthorize(ModulePermissions.Modules_Update)]
        public async Task UpdateModuleAsync(CreateUpdateModuleDTO input)
        {
            var module = await _moduleRepository.FirstOrDefaultAsync(input.Id);
            if (module == null)
            {
                throw new UserFriendlyException($"Module with ID {input.Id} not found.");
            }
            module.Title = input.Title;
            module.Description = input.Description;
            module.CourseId = input.CourseId;

            _mapper.Map<ModuleDTO>(module);

             await _moduleRepository.UpdateAsync(module);
        }

        public async Task<List<ModuleDTO>> FilterModuleByCourseId(int courseId)
        {
            var modules = await _moduleRepository.GetAll()
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            if (!modules.Any())
            {
                throw new UserFriendlyException($"No modules found for Course ID {courseId}.");
            }

             return _mapper.Map<List<ModuleDTO>>(modules);


        }

        public async Task<List<ModuleDTO>> GetModulesByKeyword(string keyword)
        {
            var module = await _moduleRepository.GetAll()
                .Where(m => m.Title.Contains(keyword) || m.Description.Contains(keyword))
                .Select(m => new ModuleDTO
                {
                    Title = m.Title,
                    Description = m.Description,
                    Id = m.Id,
                    CourseTitle = m.Course.Title,
                }).ToListAsync();

            if (!module.Any())
            {
                throw new UserFriendlyException($"No courses found with keyword '{keyword}'.");
            }

            return _mapper.Map<List<ModuleDTO>>(module);

        }


    }
}
