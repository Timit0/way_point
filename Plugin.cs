#if TOOLS
using Godot;
using System;

[Tool]
public partial class Plugin : EditorPlugin
{
    protected const string WAY_POINT_3D_NODE_NAME = "WayPoint3D";
    protected const string WAY_POINT_3D_SCRIPT_PATH = "res://addons/way_point/src/3d/WayPoint3D.cs";

    public override void _EnterTree()
    {
        Texture2D icon3D = GD.Load<Texture2D>(Icon.WAY_POINT_3D_ICON_PATH);
        CSharpScript wp3dScript = GD.Load<CSharpScript>(WAY_POINT_3D_SCRIPT_PATH);
        this.AddCustomType(WAY_POINT_3D_NODE_NAME, "Node3D", wp3dScript, icon3D);
    }

    public override void _ExitTree()
    {
        this.RemoveCustomType(WAY_POINT_3D_NODE_NAME);
    }
}
#endif
