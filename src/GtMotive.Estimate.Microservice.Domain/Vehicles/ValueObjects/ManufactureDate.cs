using System;
using System.Globalization;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    public readonly struct ManufactureDate
    {
        private readonly DateTime _value;

        public ManufactureDate(DateTime value)
        {
            if (value > DateTime.UtcNow)
            {
                throw new ArgumentException("Manufacture date cannot be in the future", nameof(value));
            }

            // Regla de negocio: no más de 5 años
            var fiveYearsAgo = DateTime.UtcNow.AddYears(-5);
            if (value < fiveYearsAgo)
            {
                throw new ArgumentException("Vehicle cannot be older than 5 years", nameof(value));
            }

            _value = value.Date;
        }

        public DateTime ToDateTime() => _value;

        public int GetAgeInYears()
        {
            var today = DateTime.UtcNow;
            var age = today.Year - _value.Year;
            if (today.DayOfYear < _value.DayOfYear)
            {
                age--;
            }
            return age;
        }

        public override string ToString() => _value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        public override bool Equals(object obj) =>
            obj is ManufactureDate other && _value.Equals(other._value);

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(ManufactureDate left, ManufactureDate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ManufactureDate left, ManufactureDate right)
        {
            return !(left == right);
        }
    }
}
