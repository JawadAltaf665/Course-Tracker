using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace CourseTracker.Authorization
{
    public class CourseTrackerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);


            var courses = context.CreatePermission(CoursePermissions.Courses, L("Courses"));
            courses.CreateChildPermission(CoursePermissions.Courses_Create, L("CreateCourse"));
            courses.CreateChildPermission(CoursePermissions.Courses_Update, L("EditCourse"));
            courses.CreateChildPermission(CoursePermissions.Courses_Delete, L("DeleteCourse"));

            var modules = context.CreatePermission(ModulePermissions.Modules, L("Modules"));
            courses.CreateChildPermission(ModulePermissions.Modules_Create, L("CreateModule"));
            courses.CreateChildPermission(ModulePermissions.Modules_Update, L("EditModule"));
            courses.CreateChildPermission(ModulePermissions.Modules_Delete, L("DeleteModule"));

            var learners = context.CreatePermission(LearnerPermissions.Learners, L("Learners"));
            courses.CreateChildPermission(LearnerPermissions.Learners_Create, L("CreateLearner"));
            courses.CreateChildPermission(LearnerPermissions.Learners_Update, L("EditLearner"));
            courses.CreateChildPermission(LearnerPermissions.Learners_Delete, L("DeleteLearner"));

            var enrollments = context.CreatePermission(EnrollmentPermissions.Enrollments, L("Enrollments"));
            courses.CreateChildPermission(EnrollmentPermissions.Enrollments_Create, L("CreateEnrollment"));
            courses.CreateChildPermission(EnrollmentPermissions.Enrollments_Update, L("EditEnrollment"));
            courses.CreateChildPermission(EnrollmentPermissions.Enrollments_Delete, L("DeleteEnrollment"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CourseTrackerConsts.LocalizationSourceName);
        }

        public static class CoursePermissions
        {
            public const string Courses = "Pages.Courses";
            public const string Courses_Create = "Pages.Courses.Create";
            public const string Courses_Update = "Pages.Courses.Update";
            public const string Courses_Delete = "Pages.Courses.Delete";
        }
        public static class ModulePermissions
        {
            public const string Modules = "Pages.Modules";
            public const string Modules_Create = "Pages.Modules.Create";
            public const string Modules_Update = "Pages.Modules.Update";
            public const string Modules_Delete = "Pages.Modules.Delete";
        }
        public static class LearnerPermissions
        {
            public const string Learners = "Pages.Learners";
            public const string Learners_Create = "Pages.Learners.Create";
            public const string Learners_Update = "Pages.Learners.Update";
            public const string Learners_Delete = "Pages.Learners.Delete";
        }

        public static class EnrollmentPermissions
        {
            public const string Enrollments = "Pages.Enrollments";
            public const string Enrollments_Create = "Pages.Enrollments.Create";
            public const string Enrollments_Update = "Pages.Enrollments.Update";
            public const string Enrollments_Delete = "Pages.Enrollments.Delete";
        }
    }
}
