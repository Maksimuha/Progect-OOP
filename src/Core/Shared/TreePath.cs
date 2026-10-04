namespace Core.Shared;

public record TreePath
{
    private const char Separator = '.';

    public string Path { get; }
    public short Depth { get; }

    private TreePath(string path, short depth)
    {
        Path = path;
        Depth = depth;
    }

    // Единая точка создания из строки (для восстановления из БД)
    public static TreePath Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Путь не может быть пустым.");

        var parts = path.Split(Separator);

        if (parts.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Неверный формат пути в дереве оргструктуры.");

        if (!parts.All(p => Guid.TryParse(p, out _)))
            throw new ArgumentException("Путь должен состоять из Guid, разделенных точкой.");

        return new TreePath(path, (short)parts.Length);
    }

    // Создание корневого пути
    public static TreePath CreateRoot(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
            throw new ArgumentException("Id корня не может быть пустым.");

        return new TreePath(departmentId.ToString(), 1);
    }

    // Формирование дочернего пути
    public TreePath AppendedChild(Guid childId)
    {
        if (childId == Guid.Empty)
            throw new ArgumentException("Id потомка не может быть пустым.");

        return new TreePath($"{Path}{Separator}{childId}", (short)(Depth + 1));
    }

    // Проверка, что текущий путь — потомок другого
    public bool IsDescendantOf(TreePath other)
    {
        if (other is null) return false;
        return Path.StartsWith(other.Path + Separator, StringComparison.Ordinal);
    }
}