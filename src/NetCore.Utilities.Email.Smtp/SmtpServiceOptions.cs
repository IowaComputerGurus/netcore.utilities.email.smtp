using System.ComponentModel.DataAnnotations;

namespace ICG.NetCore.Utilities.Email.Smtp
{
    /// <summary>
    ///     Configuration options for use with the <see cref="SmtpService" />
    /// </summary>
    public class SmtpServiceOptions
    {
        /// <summary>
        ///     The email address defining the administrator contact
        /// </summary>
        [Display(Name = "Admin Email")]
        public string AdminEmail { get; set; }

        /// <summary>
        /// An optional name for the administrative user
        /// </summary>
        [Display(Name="Admin Name")]
        public string AdminName { get; set; }

        /// <summary>
        ///     The server for outbound emails
        /// </summary>
        [Display(Name = "Server")]
        public string Server { get; set; }

        /// <summary>
        ///     The port to use for communication
        /// </summary>
        [Display(Name = "Port")]
        public int Port { get; set; }

        /// <summary>
        ///     Should this use SSL connection
        /// </summary>
        [Display(Name = "Use SSL")]
        public bool UseSsl { get; set; }

        /// <summary>
        ///     The username to use for sending
        /// </summary>
        [Display(Name = "Sender Username")]
        public string SenderUsername { get; set; }

        /// <summary>
        ///     THe password to use for sending
        /// </summary>
        [Display(Name = "Sender Password")]
        public string SenderPassword { get; set; }

        /// <summary>
        ///     If selected outbound emails will be sent with the default template unless a special template is requested
        /// </summary>
        [Display(Name = "Always Template Emails")]
        public bool AlwaysTemplateEmails { get; set; }

        /// <summary>
        ///     If selected and email sent via a non-production environment the current environment will be added as a suffix
        /// </summary>
        [Display(Name = "Add Environment Suffix")]
        public bool AddEnvironmentSuffix { get; set; }
    }
}