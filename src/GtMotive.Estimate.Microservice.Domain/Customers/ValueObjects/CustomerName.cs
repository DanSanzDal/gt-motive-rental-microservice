using System;

namespace GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects
{
    public readonly struct CustomerName
    {
        private readonly string _value;

        public CustomerName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Customer name cannot be empty", nameof(value));
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("Customer name cannot exceed 100 characters", nameof(value));
            }

            _value = value.Trim();
        }

        public override string ToString() => _value;

        public override bool Equals(object obj) =>
            obj is CustomerName other && _value.Equals(other._value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(CustomerName left, CustomerName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerName left, CustomerName right)
        {
            return !(left == right);
        }
    }
}
