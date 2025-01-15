using Godot;
using System;

public partial class DetectionZone3D : Area3D
{
    public override void _Process(double delta)
    {
        foreach (Node node in GetOverlappingBodies())
        {
            // #if DEBUG
            GD.Print(node.Name);
            // #endif
        }
    }
}