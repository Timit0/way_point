using Godot;
using System;

public partial class DetectionZone3D : Area3D
{
    protected WayPoint3D wp3dOwner {get;set;}

    public override void _Ready()
    {
        this.BodyEntered += on_body_entered;
        this.BodyExited += on_body_exited;

        wp3dOwner = GetParent() as WayPoint3D;
        base._Ready();
    }

    private void on_body_entered(Node3D body)
    {
        WP3DSignals.Instance.Emit_BodyEntered_Signal(wp3dOwner, body);
    }

    private void on_body_exited(Node3D body)
    {
        WP3DSignals.Instance.Emit_BodyExited_Signal(wp3dOwner, body);
    }
}