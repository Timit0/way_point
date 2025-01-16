#if TOOLS
using System.Collections.Generic;
using System.Linq.Expressions;
using Godot;
using Godot.Collections;

[Tool]
public partial class WP3DRoute : Node
{
    [Export]
    public string RouteName { get; set; }
    [Export]
    public bool Loopable { get; set; } = false;
    [Export]
    public Array<WayPoint3D> WayPoints { get; set; } = new Array<WayPoint3D>();
    [Export]
    public Color ColorOfLines { get; set; } = new Color(0.9922f, 0.2392f, 0.7098f, 1);

    public override void _Process(double delta)
    {
        foreach (Node node in GetChildren())
        {
            if (node is MeshInstance3D wp3d)
            {
                this.RemoveChild(wp3d);
            }
        }

        if (VerifyIfWayPointsAreOkay(WayPoints))
        {
            if (WayPoints.Count > 1)
            {
                for (int i = 0; i < WayPoints.Count - 1; i++)
                {
                    MeshInstance3D meshInstance = new MeshInstance3D();
                    DrawLine(WayPoints[i].GlobalPosition, WayPoints[i + 1].GlobalPosition, ColorOfLines, meshInstance);
                }
            }

            if (WayPoints.Count > 1)
            {
                if (Loopable)
                {
                    MeshInstance3D meshInstance = new MeshInstance3D();
                    DrawLine(WayPoints[0].GlobalPosition, WayPoints[WayPoints.Count - 1].GlobalPosition, ColorOfLines, meshInstance);
                }
            }
        }
    }

    public void DrawLine(Vector3 start, Vector3 end, Color color, MeshInstance3D meshInstance)
    {
        ImmediateMesh mesh = new ImmediateMesh();
        StandardMaterial3D standardMaterial = new StandardMaterial3D
        {
            VertexColorUseAsAlbedo = true,
            // SubsurfScatterStrength = 12
        };

        mesh.SurfaceBegin(Mesh.PrimitiveType.LineStrip);

        mesh.SurfaceSetColor(color);

        mesh.SurfaceAddVertex(start);
        mesh.SurfaceAddVertex(end);

        mesh.SurfaceEnd();

        this.AddChild(meshInstance);
        meshInstance.Mesh = mesh;
        meshInstance.MaterialOverride = standardMaterial;
    }

    protected bool VerifyIfWayPointsAreOkay(Array<WayPoint3D> wayPoints)
    {
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            if (wayPoints[i] is null || WayPoints[i + 1] is null || !WayPoints[i].IsInsideTree() || !WayPoints[i + 1].IsInsideTree())
            {
                return false;
            }
        }
        return true;
    }
}
#endif