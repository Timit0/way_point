using Godot;
using System;

public partial class WP3D : Node3D
{
    [Export]
    public CollisionShape3D CollisionShape3D { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        (CollisionShape3D.Shape as BoxShape3D).Size = new Vector3();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
