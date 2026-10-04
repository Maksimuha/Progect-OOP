using System.Text.RegularExpressions;
using Core.Shared;

namespace Core;

public class Department
{
    public DepartmentId Id { get; private set; }
    public DOnotEmptyString Name { get; private set; }
    public DOnotEmptyString Identifier { get; private set; }
    public DepartmentId? ParentId { get; private set; }
    public TreePath Path { get; private set; }
    public EntityTime LifeTime { get; private set; }
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
        return new Department(
            id,
            name,
            identifier,
            parentId: null,
            path: TreePath.CreateRoot(id.Value),
            lifeTime: new EntityTime());
    }

    public static Department CreateChild(
        DOnotEmptyString name,
        DOnotEmptyString identifier,
        Department parent)
    {
        if (parent is null)
            throw new ArgumentException("Родитель не может быть null.");
        if (parent.LifeTime.IsDeleted)
            throw new ArgumentException("Нельзя добавить ребенка в удаленное подразделение.");

        var id = DepartmentId.New();
        return new Department(
            id,
            name,
            identifier,
            parentId: parent.Id,
            path: parent.Path.AppendedChild(id.Value),
            lifeTime: new EntityTime());
    }

    public void Rename(DOnotEmptyString newName)
    {
        Name = newName;
        LifeTime.Touch();
    }

    public void Delete()
    {
        LifeTime.SoftDelete();
    }
}
