using System;
using Godot;

namespace KentingStation.Entity;

// Defined as record struct to ensure that equality comparison is done using ID value, not using object reference (like for classes)
public record struct EntityId
{
    private string _id;

    public EntityId(string entityName)
    {
        _id = entityName + "#" + Guid.NewGuid();
    }

    public EntityId()
    {
        _id = "";
    }

    public override string ToString()
    {
        return _id;
    }

    public static implicit operator string(EntityId id)
    {
        return id.ToString();
    }

    public static implicit operator StringName(EntityId id)
    {
        return id.ToString();
    }

    public static implicit operator EntityId(string strId)
    {
        return new EntityId
        {
            _id = strId
        };
    }

    public static implicit operator EntityId(StringName strId)
    {
        return new EntityId
        {
            _id = strId
        };
    }
}