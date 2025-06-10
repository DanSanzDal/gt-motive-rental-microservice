using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.DTOs.Requests
{
    public class CreateCustomerRequest
    {
        /// <summary>
        /// Customer full name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Customer email address
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Customer phone number
        /// </summary>
        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Customer driver's license number
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Expiry date of the customer's driver's license
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "License Expiry Date")]
        public DateTime LicenseExpiryDate { get; set; }
    }
}
