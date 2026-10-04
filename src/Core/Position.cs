using Core.Shared;

namespace Core;

public class Position
{
    public PositionId Id { get; private set; }
    public DOnotEmptyString Name { get; private set; }
    public DOnotEmptyString Description { get; private set; }
    public bool IsActive { get; private set; }
    public EntityTime LifeTime { get; private set; }
    
    private Position(
        PositionId id,
        DOnotEmptyString name,
        DOnotEmptyString description,
        EntityTime lifetime)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = true;
        LifeTime = lifetime;
    }
    
    public static Position Create(DOnotEmptyString name, DOnotEmptyString description)
    {
        return new Position(
            PositionId.New(),
            name,
            description,
            new EntityTime());
    }

        public void Update(DOnotEmptyString name, DOnotEmptyString description)
        {
            Name = name;
            Description = description;
            LifeTime.Touch();
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            LifeTime.Touch();
        }

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            LifeTime.Touch();
        }

        public void Delete()
        {
            LifeTime.SoftDelete();
        }
}




