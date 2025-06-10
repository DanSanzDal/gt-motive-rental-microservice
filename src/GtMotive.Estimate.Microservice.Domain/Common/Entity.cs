using System;

namespace GtMotive.Estimate.Microservice.Domain.Common
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : struct
    {
        public TId Id { get; protected set; }

        protected Entity()
        {
        }

        protected Entity(TId id)
        {
            Id = id;
        }

        public bool Equals(Entity<TId>? other)
        {
            return other is not null && (ReferenceEquals(this, other) || Id.Equals(other.Id));
        }

        public override bool Equals(object? obj)
        {
            return obj is Entity<TId> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !Equals(left, right);
        }
    }
}
