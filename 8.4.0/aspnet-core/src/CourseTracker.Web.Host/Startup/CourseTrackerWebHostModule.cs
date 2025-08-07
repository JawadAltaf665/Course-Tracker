using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using CourseTracker.Configuration;

namespace CourseTracker.Web.Host.Startup
{
    [DependsOn(
       typeof(CourseTrackerWebCoreModule))]
    public class CourseTrackerWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public CourseTrackerWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CourseTrackerWebHostModule).GetAssembly());
        }
    }
}
