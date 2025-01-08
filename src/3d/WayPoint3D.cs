#if TOOLS
using Godot;
using System;

[Tool]
public partial class WayPoint3D : Node
{
    protected const string WP3D_TSCN_PATH = "res://addons/way_point/src/3d/WP3D.tscn";

    [Export]
    public float Width { get; set; } = 15f;

    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            PackedScene pcked = ResourceLoader.Load<PackedScene>(WP3D_TSCN_PATH);
            this.AddChild(pcked.Instantiate());
        }
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        if (Engine.IsEditorHint())
        {
            this.GetChild(0).QueueFree();
        }
        base._ExitTree();
    }
}
#endif