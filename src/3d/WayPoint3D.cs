#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[Tool]
[GlobalClass, Icon(Icon.WP3D_ICON_PATH)]
public partial class WayPoint3D : Node3D
{
    [ExportGroup("Size")]
    [Export]
    public Vector3 Size { get; set; } = new Vector3(1, 1, 1);

    //Area
    public Area3D Area { get; set; } = new Area3D();
    public CollisionShape3D CollisionShape { get; set; } = new CollisionShape3D();
    public BoxShape3D BoxShape { get; set; } = new BoxShape3D();
    protected const string AREA3D_SCRIPT = "res://addons/way_point/src/3d/DetectionZone3D.cs";

    //MeshInstance
    public MeshInstance3D MeshInstance { get; set; } = new MeshInstance3D();
    public BoxMesh BoxMeshNode { get; set; } = new BoxMesh();


    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            Area.Name = "Area3D";
            this.BoxShape.Size = this.Size;

            CollisionShape.Name = "Shape";
            CollisionShape.Shape = BoxShape;

            BoxMeshNode.Size = this.Size;
            MeshInstance.Name = "MeshInstance";
            MeshInstance.Mesh = BoxMeshNode;


            CSharpScript areaScript = GD.Load<CSharpScript>(AREA3D_SCRIPT);
            Area.SetScript(areaScript);
            Area.AddChild(CollisionShape);
            this.AddChild(Area);

            this.AddChild(MeshInstance);
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            this.BoxShape.Size = this.Size;
            BoxMeshNode.Size = this.Size;
        }

        base._Process(delta);
    }

    public override void _ExitTree()
    {
        RemoveChildren();
        base._ExitTree();
    }

    protected void RemoveChildren()
    {
        foreach (Node item in this.GetChildren())
        {
            this.RemoveChild(item);
        }
    }
}
#endif