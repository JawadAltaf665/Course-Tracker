using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace CourseTracker.Localization
{
    public static class CourseTrackerLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(CourseTrackerConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(CourseTrackerLocalizationConfigurer).GetAssembly(),
                        "CourseTracker.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
