using System;

namespace GtMotive.Estimate.Microservice.Api.DTOs.Requests
{
    public class CreateRentalRequest
    {
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // public decimal DailyRate { get; set; } 
    }
}
