using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects
{
    public readonly struct RentalPeriod : IEquatable<RentalPeriod>
    {
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }

        public RentalPeriod(DateTime startDate, DateTime? endDate = null)
        {
            if (startDate == default)
            {
                throw new ArgumentException("Start date cannot be default", nameof(startDate));
            }

            if (endDate.HasValue && endDate.Value.Date <= startDate.Date.AddDays(1).AddSeconds(-1))
            {
                throw new ArgumentException("End date must be at least 1 day after start date", nameof(endDate));
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsActive()
        {
            return !EndDate.HasValue;
        }

        public TimeSpan GetDuration()
        {
            var endPoint = EndDate ?? DateTime.UtcNow;
            return endPoint - StartDate;
        }

        public int GetDurationInDays()
        {
            var endPoint = EndDate ?? DateTime.UtcNow;
            return (int)(endPoint - StartDate).TotalDays;
        }

        public RentalPeriod Complete(DateTime endDate)
        {
            return new RentalPeriod(StartDate, endDate);
        }

        public bool ContainsPeriod(DateTime date)
        {
            return date >= StartDate && (EndDate == null || date <= EndDate);
        }

        public bool Equals(RentalPeriod other)
        {
            return StartDate == other.StartDate && EndDate == other.EndDate;
        }

        public override bool Equals(object? obj) => obj is RentalPeriod other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(StartDate, EndDate);

        public static bool operator ==(RentalPeriod left, RentalPeriod right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RentalPeriod left, RentalPeriod right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return EndDate.HasValue
                ? $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}"
                : $"From {StartDate:yyyy-MM-dd} (Active)";
        }
    }
}
