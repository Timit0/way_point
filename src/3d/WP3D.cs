#if TOOLS
using Godot;
using System;

[Tool]
[GlobalClass, Icon(Icon.WP3D_ICON_PATH)]
public partial class WP3D : Node3D
{
    [Export]
    public CollisionShape3D CollisionShape3D { get; set; }

    public WPModel3D Model { get; set; } = new WPModel3D();

    public override void _ExitTree()
    {
        if (Engine.IsEditorHint())
        {
            try
            {
                if (Model.WPOwner is not null)
                {
                    PluginSignals.Instance.Emit_QueueFreePluginNode_Signal(Model.WPOwner.ToString());
                }
            }
            catch (Exception e) { };
        }
        base._ExitTree();
    }
}
#endif
