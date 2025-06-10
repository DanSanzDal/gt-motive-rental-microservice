using System;

namespace GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects
{
    public readonly struct CustomerId : IEquatable<CustomerId>
    {
        public Guid Value { get; }

        public CustomerId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("CustomerId cannot be empty", nameof(value));
            }

            Value = value;
        }

        public static CustomerId New() => new(Guid.NewGuid());

        public Guid ToGuid() => Value;

        public bool Equals(CustomerId other) => Value.Equals(other.Value);

        public override bool Equals(object? obj) => obj is CustomerId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(CustomerId customerId)
        {
            return customerId.Value;
        }

        public static bool operator ==(CustomerId left, CustomerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerId left, CustomerId right)
        {
            return !(left == right);
        }
    }
}
