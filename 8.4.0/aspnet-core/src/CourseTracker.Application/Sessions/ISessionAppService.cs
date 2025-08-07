using System.Threading.Tasks;
using Abp.Application.Services;
using CourseTracker.Sessions.Dto;

namespace CourseTracker.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
