using System.ComponentModel.DataAnnotations;

namespace Precise.Web.Models.TokenAuth
{
    public class SendTwoFactorAuthCodeModel
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Required]
        public string Provider { get; set; }
    }
}