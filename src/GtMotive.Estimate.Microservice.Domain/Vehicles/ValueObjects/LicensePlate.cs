using System;
using System.Text.RegularExpressions;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    public readonly struct LicensePlate : IEquatable<LicensePlate>
    {
        private readonly string _value;

        public LicensePlate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("License plate cannot be empty", nameof(value));
            }

            // Formato básico: letras y números (se puede ajustar según país)
            if (!Regex.IsMatch(value.Trim(), @"^[A-Z0-9]{6,8}$"))
            {
                throw new ArgumentException("Invalid license plate format", nameof(value));
            }

            _value = value.Trim().ToUpperInvariant();
        }

        public bool Equals(LicensePlate other)
        {
            return _value.Equals(other._value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj) => obj is LicensePlate other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => _value;

        public static bool operator ==(LicensePlate left, LicensePlate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LicensePlate left, LicensePlate right)
        {
            return !(left == right);
        }

        public static implicit operator string(LicensePlate licensePlate)
        {
            return licensePlate._value;
        }
    }
}
