using System;
using System.Text.RegularExpressions;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    public readonly struct VIN : IEquatable<VIN>
    {
        private readonly string _value;

        public VIN(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("VIN cannot be empty", nameof(value));
            }

            // VIN debe tener exactamente 17 caracteres alfanuméricos
            if (!Regex.IsMatch(value.Trim(), @"^[A-HJ-NPR-Z0-9]{17}$"))
            {
                throw new ArgumentException("Invalid VIN format. Must be 17 alphanumeric characters excluding I, O, Q", nameof(value));
            }

            _value = value.Trim().ToUpperInvariant();
        }

        public bool Equals(VIN other)
        {
            return _value.Equals(other._value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj) => obj is VIN other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => _value;

        public static bool operator ==(VIN left, VIN right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VIN left, VIN right)
        {
            return !(left == right);
        }

        public static implicit operator string(VIN vin)
        {
            return vin._value;
        }

        public static implicit operator VIN(string value)
        {
            return new VIN(value);
        }
    }
}
