using CourseTracker.Debugging;

namespace CourseTracker
{
    public class CourseTrackerConsts
    {
        public const string LocalizationSourceName = "CourseTracker";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "52f97ecc792e4729b8e906ddbd61a62e";
    }
}
