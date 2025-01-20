using Godot;

public partial class WP3DSignals : Singleton<WP3DSignals>
{
    private WP3DSignals(){}

    [Signal]
    public delegate void BodyEnteredEventHandler(WayPoint3D wp3d, Node3D nodeEntered);
    public void Emit_BodyEntered_Signal(WayPoint3D wp3d, Node nodeEntered)
    {
        this.EmitSignal(SignalName.BodyEntered, wp3d, nodeEntered);
    }

    [Signal]
    public delegate void BodyExitedEventHandler(WayPoint3D wp3d, Node3D nodeExited);
    public void Emit_BodyExited_Signal(WayPoint3D wp3d, Node nodeExited)
    {
        this.EmitSignal(SignalName.BodyExited, wp3d, nodeExited);
    }
}