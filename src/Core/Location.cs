using Core.Shared;

namespace Core;

public sealed class Location
{
    public LocationId Id { get; private set; }
    public DOnotEmptyString Name { get; private set; }
    public DOnotEmptyString Address { get; private set; }
    public TimeZonee TimeZone { get; private set; }
    public EntityTime LifeTime { get; private set; }

    private Location(
        LocationId id,
        DOnotEmptyString name,
        DOnotEmptyString address,
        TimeZonee timeZone,
        EntityTime lifeTime)
    {
        Id = id;
        Name = name;
        Address = address;
        TimeZone = timeZone;
        LifeTime = lifeTime;
    }

    public static Location Create(
        DOnotEmptyString name,
        DOnotEmptyString address,
        TimeZonee timeZone)
    {
        return new Location(
            LocationId.New(),
            name,
            address,
            timeZone,
            new EntityTime());
    }

    internal static Location Restore(
        LocationId id,
        DOnotEmptyString name,
        DOnotEmptyString address,
        TimeZonee timeZone,
        EntityTime lifeTime)
        => new(id, name, address, timeZone, lifeTime);

    public void Update(
        DOnotEmptyString? name = null,
        DOnotEmptyString? address = null,
        TimeZonee? timeZone = null)
    {
        EnsureNotArchived();

        bool changed = false;
        if (name is not null)     { Name = name;         changed = true; }
        if (address is not null)  { Address = address;   changed = true; }
        if (timeZone is not null) { TimeZone = timeZone; changed = true; }

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
            throw new InvalidOperationException($"Локация {Id} архивирована.");
    }
}