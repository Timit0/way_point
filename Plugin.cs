#if TOOLS
using Godot;
using System;

[Tool]
public partial class Plugin : EditorPlugin
{
    protected const string WP3D_NODE_NAME = "WayPoint3D";
    protected const string WP3D_ICON_PATH = "res://addons/way_point/src/assets/icon_3d.svg";
    protected const string WP3D_SCRIPT_PATH = "res://addons/way_point/src/3d/WayPoint3D.cs";

    public override void _EnterTree()
    {
        Texture2D icon3D = GD.Load<Texture2D>(WP3D_ICON_PATH);
        CSharpScript wp3dScript = GD.Load<CSharpScript>(WP3D_SCRIPT_PATH);
        this.AddCustomType(WP3D_NODE_NAME, "Node3D", wp3dScript, icon3D);
    }

    public override void _ExitTree()
    {
        this.RemoveCustomType(WP3D_NODE_NAME);
    }
}
#endif
