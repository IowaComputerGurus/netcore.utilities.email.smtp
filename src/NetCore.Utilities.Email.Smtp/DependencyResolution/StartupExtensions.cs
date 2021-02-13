using ICG.NetCore.Utilities.Email.Smtp;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to make DI easier
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        ///     Registers the items included in the ICG AspNetCore Utilities project for Dependency Injection
        /// </summary>
        /// <param name="services">Your existing services collection</param>
        /// <param name="configuration">The configuration instance to load settings</param>
        public static void UseIcgNetCoreUtilitiesEmailSmtp(this IServiceCollection services, IConfiguration configuration)
        {
            //Register internal services
            services.UseIcgNetCoreUtilitiesEmail(configuration);

            //Bind additional services
            services.AddTransient<IMimeKitService, MimeKitService>();
            services.AddTransient<IMimeMessageFactory, MimeMessageFactory>();
            services.AddTransient<ISmtpService, SmtpService>();
            services.Configure<SmtpServiceOptions>(configuration.GetSection(nameof(SmtpServiceOptions)));
        }
    }
}