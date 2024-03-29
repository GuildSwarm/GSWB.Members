﻿using System.ComponentModel.DataAnnotations;

namespace Members.Domain.Entities
{
    /// <summary>
    /// Represents a verification code expedited for a given member in order to support some external verification with the system. 
    /// </summary>
    /// <remarks>Used to verify RSI account by pasting this generated code for the member in the RSI profile.</remarks>
    public class VerifyCode
    {
        /// <summary>
        /// Global unique id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Discord user ID from OAuth with discord
        /// </summary>
        public required Member Member { get; set; }

        /// <summary>
        /// Code used for external member verification. Used to verify RSI account by pasting this generated code for the member in the RSI profile.
        /// </summary>
        [MaxLength(32)]
        [MinLength(32)]
        public required string Code { get; set; }

        /// <summary>
        /// The expiry date for this verification code. 
        /// </summary>
        public DateTimeOffset ExpiryDate { get; set; }

    }
}
