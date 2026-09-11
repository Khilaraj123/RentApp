using System.ComponentModel.DataAnnotations;

namespace RentApp.Application.Common.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        [Required, MinLength(32)]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int AccessTokenExpirationMinutes { get; set; } = 15;

        [Range(1, int.MaxValue)]
        public int RefreshTokenExpirationDays { get; set; } = 30;
    }
}
