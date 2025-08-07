using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using CourseTracker.EntityFrameworkCore;
using CourseTracker.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace CourseTracker.Web.Tests
{
    [DependsOn(
        typeof(CourseTrackerWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class CourseTrackerWebTestModule : AbpModule
    {
        public CourseTrackerWebTestModule(CourseTrackerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CourseTrackerWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(CourseTrackerWebMvcModule).Assembly);
        }
    }
}