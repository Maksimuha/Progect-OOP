namespace Core;

public class Department
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    // 
    public string Identifier { get; private set; } = string.Empty;

    public Guid? ParentID { get; private set; }

    public string Path { get; private set; } = string.Empty;

    public short Depth { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;


    private Department(
        Guid id,
        string name,
        string identifier,
        Guid? parentID,
        string path,
        short depth,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentID = parentID;
        Path = path;
        Depth = depth;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
