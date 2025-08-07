using Abp.Authorization;
using CourseTracker.Authorization.Roles;
using CourseTracker.Authorization.Users;

namespace CourseTracker.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
