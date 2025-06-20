﻿using System;
using System.Text.RegularExpressions;

namespace GtMotive.Estimate.Microservice.Domain.Common.ValueObjects
{
    /// <summary>
    /// Value object representing an email address.
    /// </summary>
    public sealed record Email
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be null or empty", nameof(value));

            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException("Invalid email format", nameof(value));

            Value = value.ToLowerInvariant();
        }

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string value) => new(value);

        public override string ToString() => Value;
    }
}
