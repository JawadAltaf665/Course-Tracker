using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using CourseTracker.Configuration.Dto;

namespace CourseTracker.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : CourseTrackerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
