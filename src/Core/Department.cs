using System.Text.RegularExpressions;
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

    // Depth всегда берем из Path, чтобы не было рассинхрона
    public short Depth => Path.Depth;

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
        return new Department(
            id,
            name,
            identifier,
            parentId: parent.Id,
            path: parent.Path.AppendedChild(id.Value),
            lifeTime: new EntityTime());
    }

    // Для восстановления из БД
    internal static Department Restore(
        DepartmentId id,
        DOnotEmptyString name,
        DOnotEmptyString identifier,
        DepartmentId? parentId,
        TreePath path,
        EntityTime lifeTime)
        => new(id, name, identifier, parentId, path, lifeTime);

    // Обновление полей (имя и/или идентификатор)
    public void Update(
        DOnotEmptyString? name = null,
        DOnotEmptyString? identifier = null)
    {
        EnsureNotArchived();

        bool changed = false;
        if (name is not null)       { Name = name;             changed = true; }
        if (identifier is not null) { Identifier = identifier; changed = true; }

        if (changed) LifeTime.Touch();
    }

    // Присоединение к новому родителю
    public void SetParent(Department newParent)
    {
        EnsureNotArchived();

        if (newParent is null)
            throw new ArgumentException("Родитель не может быть null.");
            
        if (newParent.Id == Id)
            throw new InvalidOperationException("Нельзя присоединить подразделение к самому себе.");
            
        if (ParentId == newParent.Id)
            throw new InvalidOperationException("Этот родитель уже назначен.");
            
        if (newParent.LifeTime.IsDeleted)
            throw new InvalidOperationException("Нельзя присоединиться к архивированному подразделению.");
            
        // Проверка на цикл (нельзя присоединить родителя к своему же потомку)
        if (newParent.Path.IsDescendantOf(Path))
            throw new InvalidOperationException("Нельзя присоединить подразделение к своему потомку (цикл).");

        ParentId = newParent.Id;
        Path = newParent.Path.AppendedChild(Id.Value);
        LifeTime.Touch();
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

