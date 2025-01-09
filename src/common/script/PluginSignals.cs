using Godot;

public partial class PluginSignals : Singleton<PluginSignals>
{
    private PluginSignals() { }

    [Signal]
    public delegate void QueueFreePluginNodeEventHandler(string WPOwnerToString);
    public void Emit_QueueFreePluginNode_Signal(string WPOwnerToString)
    {
        this.EmitSignal(SignalName.QueueFreePluginNode, WPOwnerToString);
    }
}