using System;
using System.Reflection;
using System.Text;
using Godot;
using KentingStation.Common.Util;

namespace KentingStation.Interface;

public interface IDisplayDebugInfo
{
    public Label SetupDebugInfo()
    {
        var labelScene = ResourceLoader.Load<PackedScene>("res://Scene/DebugInfo.tscn");
        var label = labelScene.Instantiate<Label>();
        AddChild(label);
        return label;
    }

    public void UpdateDebugInfo(Label debugLabel)
    {
        var sb = new StringBuilder();
        var type = GetType();

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var attribute = (DebugInfo)Attribute.GetCustomAttribute(field, typeof(DebugInfo));
            if (attribute is not null)
                sb.AppendLine($"{attribute.DisplayName ?? field.Name}: {field.GetValue(this)}");
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var attribute = (DebugInfo)Attribute.GetCustomAttribute(property, typeof(DebugInfo));
            if (attribute is not null)
                sb.AppendLine($"{attribute.DisplayName ?? property.Name}: {property.GetValue(this)}");
        }

        debugLabel.Text = sb.ToString();
    }

    public void AddChild(Node node, bool forceReadableName = false,
        Node.InternalMode @internal = 0);
}