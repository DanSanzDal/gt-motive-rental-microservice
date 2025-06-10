using System;

namespace GtMotive.Estimate.Microservice.Domain.Common
{
    public abstract class AggregateRoot<TId> : IEquatable<AggregateRoot<TId>>
        where TId : struct
    {
        public TId Id { get; protected set; }

        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TId id)
        {
            Id = id;
        }

        public bool Equals(AggregateRoot<TId>? other)
        {
            return other is not null && (ReferenceEquals(this, other) || Id.Equals(other.Id));
        }

        public override bool Equals(object? obj)
        {
            return obj is AggregateRoot<TId> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(AggregateRoot<TId>? left, AggregateRoot<TId>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AggregateRoot<TId>? left, AggregateRoot<TId>? right)
        {
            return !Equals(left, right);
        }
    }
}
