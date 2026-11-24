using Core.Entities;
using Core.Shared;

namespace Core;

public sealed class Department
{
    public DepartmentId Id { get; private set; }
    public DOnotEmptyString Name { get; private set; }
    public DOnotEmptyString Identifier { get; private set; }
    public DepartmentId? ParentId { get; private set; }
    public TreePath Path { get; private set; }
    public EntityTime LifeTime { get; private set; }
    private readonly List<DepartmentLocation> _locations = new();
    private readonly List<DepartmentPosition> _positions = new();

    private Department(
        DepartmentId id,
        DOnotEmptyString name,
        DOnotEmptyString identifier,
        DepartmentId? parentId,
        TreePath path,
        EntityTime lifeTime)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        LifeTime = lifeTime;
    }

    // Корневое подразделение
    public static Department CreateRoot(DOnotEmptyString name, DOnotEmptyString identifier)
    {
        var id = DepartmentId.New();
        var treePath = TreePath.CreateRoot(identifier);
        return Create(
            id,
            name,
            identifier,
            parentId: null,
            path: treePath,
            lifeTime: new EntityTime());
    }

    // Дочернее подразделение
    public static Department CreateChild(
        DOnotEmptyString name,
        DOnotEmptyString identifier,
        Department parent)
    {
        if (parent is null)
            throw new ArgumentException("Родитель не может быть null.");
        if (parent.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя добавить ребенка в архивированное подразделение.");

        var id = DepartmentId.New();
        var treePath = parent.Path.AppendedChild(identifier);
        return Create(
            id,
            name,
            identifier,
            parentId: parent.Id,
            path: treePath,
            lifeTime: new EntityTime());
    }

    public static Department Create(
        DepartmentId id,
        DOnotEmptyString name,
        DOnotEmptyString identifier,
        DepartmentId? parentId,
        TreePath path,
        EntityTime lifeTime)
    {
        if (id.Value == Guid.Empty)
            throw new ArgumentException("ID подразделения не может быть пустым.", nameof(id));

        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(lifeTime);

        return new Department(id, name, identifier, parentId, path, lifeTime);
    }

    public DepartmentLocation AttachLocation(Location location)
    {
        EnsureNotArchived();
        ArgumentNullException.ThrowIfNull(location);

        if (location.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя прикрепить архивированную локацию.");
        var locationexist = _locations.Any(l =>
        { 
            var idExuals = l.LocationId == location.Id;
            if (idExuals)
                return true;

            var addressEquals = l.LocationName.Value == location.Address.Value;
            if (addressEquals)
                return true;

            return false;

        });
        if (locationexist)
            throw new InvalidOperationException($"В подразделении уже есть локация с идентификатором '{location.Id}' или по адресу '{location.Address.Value}'.");
        
        var createdLocation = DepartmentLocation.Create(Id, location.Id, location.Address);
        _locations.Add(createdLocation);
        return createdLocation;
    }

    public DepartmentPosition AttachPosition(Position position, bool alreadyAttached, bool nameAlreadyUsed)
    {
        EnsureNotArchived();
        ArgumentNullException.ThrowIfNull(position);

        if (position.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя прикрепить архивированную должность.");
        var positionexist = _positions.Any(p =>
        { 
            var idExuals = p.PositionId == position.Id;
            if (idExuals)
                return true;

            var nameEquals = p.PositionName.Value == position.Name.Value;
            if (nameEquals)
                return true;

            return false;

        });
        if (positionexist)
            throw new InvalidOperationException($"В подразделении уже есть должность с идентификатором '{position.Id}' или названием '{position.Name.Value}'.");
        
        var createdPosition = DepartmentPosition.Create(Id, position.Id, position.Name);
        _positions.Add(createdPosition);
        return createdPosition;
    }

    // Обновление полей (имя и/или идентификатор)
    public void Update(
        DOnotEmptyString? name = null,
        DOnotEmptyString? identifier = null)
    {
        EnsureNotArchived();

        bool changed = false;
        if (name is not null) { Name = name; changed = true; }
        if (identifier is not null) { Identifier = identifier; changed = true; }

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
            throw new InvalidOperationException($"Подразделение {Id} архивировано. Редактирование запрещено.");
    }
}