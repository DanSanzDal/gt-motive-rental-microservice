using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects
{
    public readonly struct RentalAgreementId : IEquatable<RentalAgreementId>
    {
        private readonly Guid _value;

        public RentalAgreementId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("RentalAgreementId cannot be empty", nameof(value));
            }

            _value = value;
        }

        public static RentalAgreementId New() => new(Guid.NewGuid());

        public Guid ToGuid() => _value;

        public bool Equals(RentalAgreementId other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object? obj) => obj is RentalAgreementId other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => _value.ToString();

        public static bool operator ==(RentalAgreementId left, RentalAgreementId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RentalAgreementId left, RentalAgreementId right)
        {
            return !(left == right);
        }

        public static implicit operator Guid(RentalAgreementId rentalId)
        {
            return rentalId._value;
        }

        public static implicit operator RentalAgreementId(Guid value)
        {
            return new(value);
        }
    }
}
