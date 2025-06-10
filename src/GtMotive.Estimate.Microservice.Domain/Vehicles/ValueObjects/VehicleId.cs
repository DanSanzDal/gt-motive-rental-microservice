using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    public readonly struct VehicleId : IEquatable<VehicleId>
    {
        private readonly Guid _value;

        public VehicleId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("VehicleId cannot be empty", nameof(value));
            }

            _value = value;
        }

        public static VehicleId New() => new(Guid.NewGuid());

        public Guid ToGuid() => _value;

        public bool Equals(VehicleId other) => _value.Equals(other._value);

        public override bool Equals(object? obj) => obj is VehicleId other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => _value.ToString();

        public static bool operator ==(VehicleId left, VehicleId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VehicleId left, VehicleId right)
        {
            return !(left == right);
        }

        public static implicit operator Guid(VehicleId vehicleId)
        {
            return vehicleId._value;
        }

        public static implicit operator VehicleId(Guid value)
        {
            return new(value);
        }
    }
}
