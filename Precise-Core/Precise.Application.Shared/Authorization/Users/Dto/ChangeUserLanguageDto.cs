using System.ComponentModel.DataAnnotations;

namespace Precise.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
