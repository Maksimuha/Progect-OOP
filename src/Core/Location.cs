using Core.Shared;

namespace Core;

public class Location
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
        TimeZonee timeZone)   // ← было TimeZonee
    {
        return new Location(
            LocationId.New(),
            name,
            address,
            timeZone,
            new EntityTime());
    }

    public void Update(
        DOnotEmptyString name,
        DOnotEmptyString address,
        TimeZonee timeZone)
    {
        Name = name;
        Address = address;
        TimeZone = timeZone;
        LifeTime.Touch();
    }

    public void Delete() => LifeTime.SoftDelete();
}
