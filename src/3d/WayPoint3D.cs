#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[Tool]
[GlobalClass, Icon(Icon.WAY_POINT_3D_ICON_PATH)]
public partial class WayPoint3D : Node3D
{
    protected Vector3 size { get; set; } = new Vector3(1, 1, 1);
    [Export]
    public Vector3 Size
    {
        get
        {
            return this.size;
        }
        set
        {
            this.size = value;
            this.BoxShape.Size = this.size;
            this.BoxMeshNode.Size = this.size;
        }
    }

    //Area
    [ExportCategory("Area3D")]
    [Export(PropertyHint.Layers3DPhysics)]
    public uint Layer { get; set; } = 1;
    [Export(PropertyHint.Layers3DPhysics)]
    public uint Mask { get; set; } = 1;

    public DetectionZone3D Area { get; set; } = new DetectionZone3D();
    public CollisionShape3D CollisionShape { get; set; } = new CollisionShape3D();
    public BoxShape3D BoxShape { get; set; } = new BoxShape3D();
    protected const string AREA3D_SCRIPT = "res://addons/way_point/src/3d/DetectionZone3D.cs";

    //MeshInstance
    public MeshInstance3D MeshInstance { get; set; } = new MeshInstance3D();
    public BoxMesh BoxMeshNode { get; set; } = new BoxMesh();

    //Labels
    public Label3D Label3DNumber {get;set;} = new Label3D();
    public bool LabelCanBeSet {get;set;} = true;


    public override void _EnterTree()
    {
        SetUpArea();
        if(Engine.IsEditorHint())
        {
            SetUpMeshInstance();
            SetUpLabels();
        }
    }

    public override void _Process(double delta)
    {
        if(Engine.IsEditorHint())
        {
            Label3DNumber.Text = "";
        }
        base._Process(delta);
    }

    public override void _ExitTree()
    {
        RemoveChildrenOf(Area);
        RemoveChildrenOf(this);
        base._ExitTree();
    }

    protected void RemoveChildrenOf(Node node)
    {
        foreach (Node item in node.GetChildren())
        {
            node.RemoveChild(item);
        }
    }

    public void SetUpArea()
    {
        this.AddChild(Area);
        Area.AddChild(CollisionShape);
        Area.CollisionLayer = this.Layer;
        Area.CollisionMask = this.Mask;

        CollisionShape.Shape = BoxShape;
        BoxShape.Size = this.Size;
    }

    public void SetUpMeshInstance()
    {
        this.AddChild(MeshInstance);
        MeshInstance.Mesh = BoxMeshNode;
        BoxMeshNode.Size = this.Size;
    }

    public void SetUpLabels()
    {
        this.AddChild(Label3DNumber);
        Label3DNumber.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;
        Label3DNumber.NoDepthTest = true;
        Label3DNumber.TextureFilter = BaseMaterial3D.TextureFilterEnum.Nearest;
        Label3DNumber.PixelSize = 0.06f;
    }
}
#endif