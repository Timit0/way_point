#if TOOLS
using Godot;
using System;

[Tool]
public partial class Plugin : EditorPlugin
{
    protected const string WAY_POINT_3D_NAME = "WayPoint3D";
    protected const string WAY_POINT_3D_SCRIPT_PATH = "res://addons/way_point/src/3d/WayPoint3D.cs";

    protected const string WAY_POINT_3D_ROUTE_NAME = "WP3DRoute";
    protected const string WAY_POINT_3D_ROUTE_NAME_SCRIPT_PATH = "res://addons/way_point/src/3d/WP3DRoute.cs";

    public override void _EnterTree()
    {
        Texture2D wp3dIcon = GD.Load<Texture2D>(Icon.WAY_POINT_3D_ICON_PATH);
        CSharpScript wp3dScript = GD.Load<CSharpScript>(WAY_POINT_3D_SCRIPT_PATH);
        this.AddCustomType(WAY_POINT_3D_NAME, "Node3D", wp3dScript, wp3dIcon);

        Texture2D wp3dRouteIcon = GD.Load<Texture2D>(Icon.WAY_POINT_3D_ROUTE_ICON_PATH);
        CSharpScript wp3dRouteScript = GD.Load<CSharpScript>(WAY_POINT_3D_ROUTE_NAME_SCRIPT_PATH);
        this.AddCustomType(WAY_POINT_3D_ROUTE_NAME, "Node", wp3dRouteScript, wp3dRouteIcon);
    }

    public override void _ExitTree()
    {
        this.RemoveCustomType(WAY_POINT_3D_NAME);
        this.RemoveCustomType(WAY_POINT_3D_ROUTE_NAME);
    }
}
#endif