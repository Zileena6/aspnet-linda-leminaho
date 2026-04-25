namespace CoreFitness.Domain.Entities.Common;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public byte[] RowVersion { get; private set; } = default!;

    protected BaseEntity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        RowVersion = new byte[8];
    }

    public void UpdateTimeStamp() => UpdatedAt = DateTimeOffset.UtcNow;
}
