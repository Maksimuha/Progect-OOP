namespace Core;

public class Location
{
    public string Address { get; private set; } = string.Empty;


    public string Name { get; private set; } = string.Empty;

    public string Timezone { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    private Location(
        string address,
        string name,
        string timezone,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Address = address;
        Name = name;
        Timezone = timezone;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

}
