namespace Account.Core.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    
    protected Entity() { }
    
    protected Entity(Guid id)
    {
        Id = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return other.Id == this.Id;
    }
    
    public static bool operator ==(Entity left, Entity right)
    {
        if (Object.Equals(left, null))
            return (Object.Equals(right, null)) ? true : false;
        else
            return left.Equals(right);
    }
    
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}