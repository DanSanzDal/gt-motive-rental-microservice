using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects
{
    public readonly struct RentalId
    {
        private readonly Guid _value;

        public RentalId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("RentalId cannot be empty", nameof(value));
            }

            _value = value;
        }

        public static RentalId New() => new(Guid.NewGuid());

        public Guid ToGuid() => _value;

        public override string ToString() => _value.ToString();

        public override bool Equals(object obj) =>
            obj is RentalId other && _value.Equals(other._value);

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(RentalId left, RentalId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RentalId left, RentalId right)
        {
            return !(left == right);
        }
    }
}
