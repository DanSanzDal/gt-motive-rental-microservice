using System;
using System.Text.RegularExpressions;

namespace GtMotive.Estimate.Microservice.Domain.Common.ValueObjects
{
    /// <summary>
    /// Value object representing a phone number.
    /// </summary>
    public sealed record PhoneNumber
    {
        private static readonly Regex PhoneRegex = new(
            @"^\+?[1-9]\d{1,14}$",
            RegexOptions.Compiled);

        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be null or empty", nameof(value));

            var cleanValue = value.Replace(" ", "").Replace("-", "");

            if (!PhoneRegex.IsMatch(cleanValue))
                throw new ArgumentException("Invalid phone number format", nameof(value));

            Value = cleanValue;
        }

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
        public static implicit operator PhoneNumber(string value) => new(value);

        public override string ToString() => Value;
    }
}
