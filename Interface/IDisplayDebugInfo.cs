using System;
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
        var type = GetType();
        var fields = type.GetFields();
        var sb = new StringBuilder();

        foreach (var field in fields)
        {
            var attribute = (DebugInfo)Attribute.GetCustomAttribute(field, typeof(DebugInfo));
            if (attribute is not null)
                sb.AppendLine($"{attribute.DisplayName ?? field.Name}: {field.GetValue(this)}");
        }

        debugLabel.Text = sb.ToString();
    }

    public void AddChild(Node node, bool forceReadableName = false,
        Node.InternalMode @internal = 0);
}