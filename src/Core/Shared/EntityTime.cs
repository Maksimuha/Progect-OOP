namespace Core.Shared;

public record EntityTime
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public EntityTime()
    {
        var dateTime = DateTime.UtcNow;
        CreatedAt = dateTime;
        DeletedAt = null;
        UpdatedAt = dateTime;
    }
    private EntityTime(DateTime createdAt, DateTime? deletedAt, DateTime updatedAt)
    {
        CreatedAt = createdAt;
        DeletedAt = deletedAt;
        UpdatedAt = updatedAt;
    }
    public static EntityTime Create(DateTime createdAt, DateTime updatedAt, DateTime? deletedAt = null)
    {
        if (createdAt == default)
            throw new ArgumentException("Дата создания не может быть минимальной.");
        if (updatedAt == default)
            throw new ArgumentException("Дата обновления не может быть минимальной.");
        if (updatedAt < createdAt)
            throw new ArgumentException("Дата обновления не может быть раньше даты создания.");
        if (deletedAt.HasValue && deletedAt.Value < createdAt)
            throw new ArgumentException("Дата удаления не может быть раньше даты создания.");

        return new EntityTime
        {
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }

    public bool IsDeleted => DeletedAt.HasValue;

    public void Touch()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Нельзя изменять удаленную сущность.");
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (IsDeleted) return;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DeletedAt.Value;
    }

    public void Restore()
    {
        if (!IsDeleted) return;
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
