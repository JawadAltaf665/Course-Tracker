using System.ComponentModel.DataAnnotations;

namespace CourseTracker.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}