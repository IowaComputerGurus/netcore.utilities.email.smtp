﻿using System.ComponentModel.DataAnnotations;

namespace ICG.NetCore.Utilities.Email
{
    /// <summary>
    ///     Configuration options for use with the <see cref="SmtpService" />
    /// </summary>
    public class SmtpServiceOptions
    {
        /// <summary>
        /// The email address defining the administrator contact
        /// </summary>
        [Display(Name = "Admin Email")]
        public string AdminEmail { get; set; }

        /// <summary>
        /// The server for outbound emails
        /// </summary>
        [Display(Name = "Server")]
        public string Server { get; set; }

        /// <summary>
        /// The port to use for communication
        /// </summary>
        [Display(Name = "Port")]
        public int Port { get; set; }

        /// <summary>
        /// Should this use SSL connection
        /// </summary>
        [Display(Name = "Use SSL")]
        public bool UseSsl { get; set; }

        /// <summary>
        /// The username to use for sending
        /// </summary>
        [Display(Name = "Sender Username")]
        public string SenderUsername { get; set; }

        /// <summary>
        /// THe password to use for sending
        /// </summary>
        [Display(Name = "Sender Password")]
        public string SenderPassword { get; set; }
    }
}