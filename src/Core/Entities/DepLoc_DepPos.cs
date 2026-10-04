using Core.Shared;

namespace Core.Entities;

public class DepartmentLocation
{
    public DepartmentId DepartmentId { get; private set; }
    public LocationId LocationId { get; private set; }
    public EntityTime LifeTime { get; private set; }

    private DepartmentLocation(DepartmentId departmentId, LocationId locationId, EntityTime lifeTime)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
        LifeTime = lifeTime;
    }

    public static DepartmentLocation Create(DepartmentId departmentId, LocationId locationId)
    {
        // departmentId/locationId уже не могут быть пустыми — их защищает тип
        return new DepartmentLocation(departmentId, locationId, new EntityTime());
    }

    public void Delete() => LifeTime.SoftDelete();
}
public sealed class DepartmentPosition
{
    public DepartmentId DepartmentId { get; private set; }
    public PositionId PositionId { get; private set; }
    public EntityTime LifeTime { get; private set; }

    private DepartmentPosition(DepartmentId departmentId, PositionId positionId, EntityTime lifeTime)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
        LifeTime = lifeTime;
    }

    public static DepartmentPosition Create(DepartmentId departmentId, PositionId positionId)
    {
        return new DepartmentPosition(departmentId, positionId, new EntityTime());
    }

    public void Delete() => LifeTime.SoftDelete();
}