using Abp.Application.Services;
using CourseTracker.Learners_.Dtos;
using CourseTracker.Modules.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Learners_
{
    public interface ILearnerAppService: IApplicationService
    {
        Task<List<LearnerDto>> GetAllLearnersAsync();
        Task<LearnerDto> GetLearnerByIdAsync(int id);
        Task CreateLearnerAsync(CreateUpdateLearnerDTO input);
        Task UpdateLearnerAsync(CreateUpdateLearnerDTO input);
        Task DeleteLearnerAsync(int id);
        Task<List<LearnerDto>> GetLearnersByKeyword(string keyword);
    }
}
