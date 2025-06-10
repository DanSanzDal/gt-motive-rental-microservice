using System;

namespace GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects
{
    public readonly struct DrivingLicense : IEquatable<DrivingLicense>
    {
        public string Value { get; }
        public DateTime ExpiryDate { get; }

        public DrivingLicense(string value, DateTime expiryDate)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Driving license cannot be empty", nameof(value));
            }

            if (expiryDate <= DateTime.Now)
            {
                throw new ArgumentException("Driving license must not be expired", nameof(expiryDate));
            }

            Value = value.Trim().ToUpperInvariant();
            ExpiryDate = expiryDate;
        }

        public bool IsExpired => DateTime.Now > ExpiryDate;
        public bool IsValid => !IsExpired;

        public bool Equals(DrivingLicense other) =>
            Value.Equals(other.Value, StringComparison.Ordinal) &&
            ExpiryDate.Equals(other.ExpiryDate);

        public override bool Equals(object? obj) => obj is DrivingLicense other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Value, ExpiryDate);

        public override string ToString() => $"{Value} (expires: {ExpiryDate:yyyy-MM-dd})";

        public static bool operator ==(DrivingLicense left, DrivingLicense right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DrivingLicense left, DrivingLicense right)
        {
            return !(left == right);
        }
    }
}
