using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.DTOs.Requests
{
    public class CreateVehicleRequest
    {
        /// <summary>
        /// Vehicle Identification Number (VIN) - 17 characters
        /// </summary>
        [Required]
        [StringLength(17, MinimumLength = 17)]
        public string VIN { get; set; }

        /// <summary>
        /// License plate number
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Vehicle brand
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        /// <summary>
        /// Vehicle model
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        /// <summary>
        /// Vehicle manufacturing year
        /// </summary>
        [Required]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; }

        /// <summary>
        /// Manufacture date (cannot be older than 5 years)
        /// </summary>
        [Required]
        public DateTime ManufactureDate { get; set; }
    }
}
