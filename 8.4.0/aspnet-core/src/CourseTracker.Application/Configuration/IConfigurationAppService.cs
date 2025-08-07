using System.Threading.Tasks;
using CourseTracker.Configuration.Dto;

namespace CourseTracker.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
