using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    public readonly struct VehicleSpecs : IEquatable<VehicleSpecs>
    {
        public string Make { get; }
        public string Model { get; }
        public int Year { get; }
        public DateTime ManufactureDate { get; }
        public VIN VIN { get; }

        public VehicleSpecs(string make, string model, int year, DateTime manufactureDate, VIN vin)
        {
            if (string.IsNullOrWhiteSpace(make))
            {
                throw new ArgumentException("Make cannot be empty", nameof(make));
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Model cannot be empty", nameof(model));
            }

            if (year < 1900 || year > DateTime.Now.Year + 1)
            {
                throw new ArgumentException("Invalid year", nameof(year));
            }

            if (manufactureDate > DateTime.UtcNow)
            {
                throw new ArgumentException("Manufacture date cannot be in the future", nameof(manufactureDate));
            }

            Make = make.Trim();
            Model = model.Trim();
            Year = year;
            ManufactureDate = manufactureDate;
            VIN = vin;
        }

        public bool IsEligibleForFleet()
        {
            var fiveYearsAgo = DateTime.UtcNow.AddYears(-5);
            return ManufactureDate >= fiveYearsAgo;
        }

        public bool Equals(VehicleSpecs other) =>
            Make == other.Make &&
            Model == other.Model &&
            Year == other.Year &&
            ManufactureDate == other.ManufactureDate &&
            VIN == other.VIN;

        public override bool Equals(object? obj) => obj is VehicleSpecs other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Make, Model, Year, ManufactureDate, VIN);

        public static bool operator ==(VehicleSpecs left, VehicleSpecs right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VehicleSpecs left, VehicleSpecs right)
        {
            return !(left == right);
        }

        public override string ToString() => $"{Make} {Model} ({Year}) - VIN: {VIN}";
    }
}
