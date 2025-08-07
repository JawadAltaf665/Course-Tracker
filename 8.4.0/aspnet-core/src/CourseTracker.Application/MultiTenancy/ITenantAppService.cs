using Abp.Application.Services;
using CourseTracker.MultiTenancy.Dto;

namespace CourseTracker.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

