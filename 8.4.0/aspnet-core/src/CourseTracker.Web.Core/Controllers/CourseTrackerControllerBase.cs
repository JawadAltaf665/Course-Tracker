using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace CourseTracker.Controllers
{
    public abstract class CourseTrackerControllerBase: AbpController
    {
        protected CourseTrackerControllerBase()
        {
            LocalizationSourceName = CourseTrackerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
