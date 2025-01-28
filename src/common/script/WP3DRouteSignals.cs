using Godot;

public partial class WP3DRouteSignals : Singleton<WP3DRouteSignals>
{
    private WP3DRouteSignals() { }

    [Signal]
    public delegate void LapCompletedEventHandler(WP3DRoute wp3dRoute, Node3D node);
    public void Emit_LapCompleted_Signal(WP3DRoute wp3dRoute, Node3D node)
    {
        this.EmitSignal(SignalName.LapCompleted, wp3dRoute, node);
    }
}