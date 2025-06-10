using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    public readonly struct VehicleModel
    {
        private readonly string _value;

        public VehicleModel(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Vehicle model cannot be empty", nameof(value));

            if (value.Length > 50)
                throw new ArgumentException("Vehicle model cannot exceed 50 characters", nameof(value));

            _value = value.Trim();
        }

        public override string ToString() => _value;

        public override bool Equals(object obj) =>
            obj is VehicleModel other && _value.Equals(other._value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(VehicleModel left, VehicleModel right) => left.Equals(right);
        public static bool operator !=(VehicleModel left, VehicleModel right) => !(left == right);
    }
}
