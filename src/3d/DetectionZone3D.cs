#if TOOLS
using Godot;
using System;

[Tool]
public partial class DetectionZone3D : Area3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // foreach (Node node in GetOverlappingBodies())
        // {
        //     GD.Print(node.Name);
        // }
        GD.Print("A");
    }
}
#endif