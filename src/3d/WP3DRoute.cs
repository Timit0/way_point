#if TOOLS
using System.Collections.Generic;
using Godot;
using Godot.Collections;

[Tool]
public partial class WP3DRoute : Node
{
    [Export]
    public string RouteName {get;set;}

    protected Array<WayPoint3D> wayPoints3D = new Array<WayPoint3D>();

    [Export]
    public Array<WayPoint3D> WayPoints
    {
        get
        {
            return wayPoints3D;
        }
        set
        {
            wayPoints3D = value;
        }
    }

    List<MeshInstance3D> meshInstanceList {get;set;} = new List<MeshInstance3D>();

    public override void _Ready()
    {
        // GD.Print("A");
        // DrawLine(Vector3.Left, Vector3.Forward);

        base._Ready();
    }

    public override void _Process(double delta)
    {
        meshInstanceList.ForEach(x => this.RemoveChild(x));
        meshInstanceList = new List<MeshInstance3D>();

        if(WayPoints.Count > 1)
        {
            for (int i = 0; i < WayPoints.Count-1; i++)
            {
                if(WayPoints[i+1] is not null)
                {
                    MeshInstance3D meshInstance = new MeshInstance3D();
                    DrawLine(wayPoints3D[i].GlobalPosition, WayPoints[i+1].GlobalPosition, meshInstance);
                    meshInstanceList.Add(new MeshInstance3D());
                }
            }
        }
    }

    public void DrawLine(Vector3 start, Vector3 end, MeshInstance3D meshInstance)
    {
        var mesh = new ImmediateMesh();

        mesh.SurfaceBegin(Mesh.PrimitiveType.LineStrip);

        mesh.SurfaceAddVertex(start);
        mesh.SurfaceAddVertex(end);

        mesh.SurfaceEnd();
        
        this.AddChild(meshInstance);
        meshInstance.Mesh = mesh;
    }
}
#endif