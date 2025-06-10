namespace GtMotive.Estimate.Microservice.Infrastructure.Configuration
{
    public sealed class AppSettings
    {
        public const string SectionName = "AppSettings";

        public string JwtAuthority { get; set; } = string.Empty;
        public string PathBase { get; set; } = "/";
    }
}
