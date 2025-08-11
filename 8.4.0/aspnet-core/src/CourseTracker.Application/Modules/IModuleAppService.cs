using Abp.Application.Services;
using CourseTracker.Modules.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Modules
{
    public interface IModuleAppService: IApplicationService
    {
        Task<List<ModuleDTO>> GetAllModulesAsync();
        Task<ModuleDTO> GetModuleByIdAsync(int id);
        Task CreateModuleAsync(CreateUpdateModuleDTO input);
        Task UpdateModuleAsync(CreateUpdateModuleDTO input);
        Task DeleteModuleAsync(int id);

        Task<List<ModuleDTO>> FilterModuleByCourseId(int courseId);
    }
}
