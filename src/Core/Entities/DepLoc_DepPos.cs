using Core.Shared;

namespace Core.Entities;

public class DepartmentLocation
{
    public DepartmentId DepartmentId { get; private set; }
    public LocationId LocationId { get; private set; }
    public EntityTime LifeTime { get; private set; }
    public DOnotEmptyString LocationName { get; private set; }

    private DepartmentLocation(DepartmentId departmentId, LocationId locationId, EntityTime lifeTime, DOnotEmptyString locationName)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
        LifeTime = lifeTime;
        LocationName = locationName;
    }

    public static DepartmentLocation Create(DepartmentId departmentId, LocationId locationId, DOnotEmptyString locationName)
    {
        // departmentId/locationId уже не могут быть пустыми — их защищает тип
        return new DepartmentLocation(departmentId, locationId, new EntityTime(), locationName);
    }

    public void Delete() => LifeTime.SoftDelete();
}
public sealed class DepartmentPosition
{
    public DepartmentId DepartmentId { get; private set; }
    public PositionId PositionId { get; private set; }
    public EntityTime LifeTime { get; private set; }
    public DOnotEmptyString PositionName { get; private set; }

    private DepartmentPosition(
        DepartmentId departmentId,
        PositionId positionId,
        EntityTime lifeTime,
        DOnotEmptyString positionName)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
        LifeTime = lifeTime;
        PositionName = positionName;
    }

    public static DepartmentPosition Create(
        DepartmentId departmentId,
        PositionId positionId,
        DOnotEmptyString positionName)
    {
        ArgumentNullException.ThrowIfNull(positionName);
        return new DepartmentPosition(departmentId, positionId, new EntityTime(), positionName);
    }

    public void Delete() => LifeTime.SoftDelete();
}