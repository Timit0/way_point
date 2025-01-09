#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[Tool]
public partial class WayPoint3D : Node3D
{
    protected const string WP3D_TSCN_PATH = "res://addons/way_point/src/3d/WP3D.tscn";

    public Dictionary<string, bool> Essentials { get; set; } = new Dictionary<string, bool>
    {
        {nameof(WP3D), false},
    };

    public WP3D WP3DNode { get; set; }

    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            WP3D wp3d = new WP3D();
            UpdateEssentials();
            if (EssentialsChildrenExist())
            {
                foreach (Node node in GetChildren())
                {
                    if (node is WP3D wp3dNode)
                    {
                        wp3d = wp3dNode;
                    }
                }
                // SetUPChild(wp3d);
                WP3DNode = wp3d;
                return;
            }

            PluginSignals.Instance.QueueFreePluginNode += on_queue_free_plugin_node;

            PackedScene pcked = ResourceLoader.Load<PackedScene>(WP3D_TSCN_PATH);
            wp3d = pcked.Instantiate<WP3D>();
            this.AddChild(wp3d);
            SetUPChild(wp3d);
            WP3DNode = wp3d;
        }
        base._EnterTree();
    }


    public override void _ExitTree()
    {
        if (Engine.IsEditorHint())
        {
            PluginSignals.Instance.QueueFreePluginNode -= on_queue_free_plugin_node;

            RemoveChildren();
            WP3DNode = null;
            PluginSignals.Instance.Emit_QueueFreePluginNode_Signal(this.Name);
        }
        base._ExitTree();
    }

    private void on_queue_free_plugin_node(string WPOwnerToString)
    {
        if (WPOwnerToString == this.ToString())
        {
            this.RemoveChildren();
            this.QueueFree();
        }
    }

    protected void UpdateEssentials()
    {
        foreach (Node node in GetChildren())
        {
            foreach (KeyValuePair<string, bool> item in Essentials)
            {
                if (item.Key == node.GetType().ToString())
                {
                    Essentials[node.GetType().ToString()] = true;
                }
            }
        }
    }

    protected void SetUPChild(WP3D wp3d)
    {
        wp3d.Owner = GetTree().EditedSceneRoot;
        wp3d.Model.WPOwner = this;
    }

    protected bool EssentialsChildrenExist()
    {
        foreach (KeyValuePair<string, bool> item in Essentials)
        {
            if (!item.Value)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveChildren()
    {
        foreach (Node node in GetChildren())
        {
            node.QueueFree();
        }
    }
}
#endif