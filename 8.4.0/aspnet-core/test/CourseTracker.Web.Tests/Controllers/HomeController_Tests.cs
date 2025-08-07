using System.Threading.Tasks;
using CourseTracker.Models.TokenAuth;
using CourseTracker.Web.Controllers;
using Shouldly;
using Xunit;

namespace CourseTracker.Web.Tests.Controllers
{
    public class HomeController_Tests: CourseTrackerWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}