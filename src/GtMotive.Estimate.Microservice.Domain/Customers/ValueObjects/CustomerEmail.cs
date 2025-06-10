using System;
using System.Text.RegularExpressions;

namespace GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects
{
    public readonly struct CustomerEmail
    {
        private readonly string _value;

        // Regex compilado estático para mejor rendimiento
        private static readonly Regex EmailValidationRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public CustomerEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email cannot be empty", nameof(value));
            }

            if (!IsValidEmail(value))
            {
                throw new ArgumentException("Invalid email format", nameof(value));
            }

            _value = value.ToLowerInvariant();
        }

        private static bool IsValidEmail(string email) =>
            !string.IsNullOrEmpty(email) && EmailValidationRegex.IsMatch(email);

        public override string ToString() => _value;

        public override bool Equals(object obj) =>
            obj is CustomerEmail other && _value.Equals(other._value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(CustomerEmail left, CustomerEmail right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerEmail left, CustomerEmail right)
        {
            return !(left == right);
        }

        public static implicit operator string(CustomerEmail email)
        {
            return email._value;
        }
    }
}
