using Core.Shared;

namespace Core;

public sealed class Position
{
    public PositionId Id { get; private set; }
    public DOnotEmptyString Name { get; private set; }
    public DOnotEmptyString Description { get; private set; }
    public EntityTime LifeTime { get; private set; }

    private Position(
        PositionId id,
        DOnotEmptyString name,
        DOnotEmptyString description,
        EntityTime lifeTime)
    {
        Id = id;
        Name = name;
        Description = description;
        LifeTime = lifeTime;
    }

    public static Position Create(
        DOnotEmptyString name,
        DOnotEmptyString description)
    {
        return new Position(
            PositionId.New(),
            name,
            description,
            new EntityTime());
    }

    internal static Position Restore(
        PositionId id,
        DOnotEmptyString name,
        DOnotEmptyString description,
        EntityTime lifeTime)
        => new(id, name, description, lifeTime);

    public void Update(
        DOnotEmptyString? name = null,
        DOnotEmptyString? description = null)
    {
        EnsureNotArchived();

        bool changed = false;
        if (name is not null)        { Name = name;               changed = true; }
        if (description is not null) { Description = description; changed = true; }

        if (changed) LifeTime.Touch();
    }

    public void Archive()
    {
        if (LifeTime.IsDeleted) return;
        LifeTime.SoftDelete();
    }

    private void EnsureNotArchived()
    {
        if (LifeTime.IsDeleted)
            throw new InvalidOperationException($"Должность {Id} архивирована.");
    }
}




