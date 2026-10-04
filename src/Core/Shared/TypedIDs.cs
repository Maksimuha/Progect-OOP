namespace Core.Shared;

public readonly record struct DepartmentId
{
    public Guid Value { get; }

    public DepartmentId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(nameof(DepartmentId), "DepartmentId не может быть пустым");
        Value = value;
    }
    public static DepartmentId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(DepartmentId id) => id.Value;
}

public readonly record struct LocationId
{
    public Guid Value { get; }

    public LocationId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(nameof(LocationId), "LocationId не может быть пустым.");
        Value = value;
    }
    public static LocationId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(LocationId id) => id.Value;
}

public readonly record struct PositionId
{
    public Guid Value { get; }
    
    public PositionId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(nameof(PositionId), "PositionId не может быть пустым.");
        Value = value;
    }
    public static PositionId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(PositionId id) => id.Value;
}