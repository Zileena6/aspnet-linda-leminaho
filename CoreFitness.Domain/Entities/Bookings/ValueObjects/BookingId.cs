using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.Bookings.ValueObjects;

public readonly record struct BookingId
{
    public Guid Value { get; }

    public BookingId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static BookingId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}