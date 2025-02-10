using System;
using Godot;

namespace CozyGame.scene;

public class EntityId
{
    private string _id;

    public EntityId(string entityName)
    {
        _id = entityName + "#" + Guid.NewGuid();
    }

    private EntityId()
    {
    }

    public override string ToString()
    {
        return _id;
    }

    public static implicit operator string(EntityId id)
    {
        if (id == null)
            return null;
        return id.ToString();
    }

    public static implicit operator StringName(EntityId id)
    {
        if (id == null)
            return null;
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